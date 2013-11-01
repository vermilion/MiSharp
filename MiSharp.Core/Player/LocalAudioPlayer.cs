using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using DeadDog.Audio;
using MiSharp.Core.Player.Exceptions;
using MiSharp.Core.Player.Input;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Rareform.Validation;
using Sharpen.Lang;

namespace MiSharp.Core.Player
{
    public class LocalAudioPlayer : AudioPlayer
    {
        public delegate void PlaybackEventHandler(PlaybackEventArgs args);

        private readonly object _playerLock = new object();
        public List<IInputFileFormatPlugin> InputFileFormats;
        private WaveStream _inputStream;
        private bool _isLoaded;
        private Action<float> _setVolumeDelegate;
        private float _volume;
        private IWavePlayer _wavePlayer;

        public LocalAudioPlayer()
        {
            InputFileFormats = new List<IInputFileFormatPlugin>
                {
                    new AiffInputFilePlugin(),
                    new Mp3InputFilePlugin(),
                    new WaveInputFilePlugin()
                };

            CurrentTimeChanged = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                           .Select(x => CurrentTime)
                                           .DistinctUntilChanged(x => x.TotalSeconds);

            PlaybackStateChanged = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                             .Select(x => PlaybackState)
                                             .DistinctUntilChanged(x => x);

            TotalTimeChanged = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                         .Select(x => TotalTime)
                                         .DistinctUntilChanged(x => x.TotalSeconds);
        }

        public IObservable<TimeSpan> CurrentTimeChanged { get; private set; }
        public IObservable<TimeSpan> TotalTimeChanged { get; private set; }
        public IObservable<AudioPlayerState> PlaybackStateChanged { get; private set; }


        public override TimeSpan CurrentTime
        {
            get { return _inputStream == null ? TimeSpan.Zero : _inputStream.CurrentTime; }
            set { if (_inputStream != null) _inputStream.CurrentTime = value; }
        }

        public override AudioPlayerState PlaybackState
        {
            get
            {
                if (_wavePlayer != null)
                {
                    // We map the NAudio playbackstate to our own playback state,
                    // so that the NAudio API is not exposed outside of this class.
                    switch (_wavePlayer.PlaybackState)
                    {
                        case NAudio.Wave.PlaybackState.Stopped:
                            return AudioPlayerState.Stopped;
                        case NAudio.Wave.PlaybackState.Playing:
                            return AudioPlayerState.Playing;
                        case NAudio.Wave.PlaybackState.Paused:
                            return AudioPlayerState.Paused;
                    }
                }

                return AudioPlayerState.None;
            }
        }

        public override TimeSpan TotalTime
        {
            get { return _isLoaded ? _inputStream.TotalTime : TimeSpan.Zero; }
        }

        public override float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;

                if (_inputStream != null)
                {
                    if (_setVolumeDelegate != null)
                    {
                        _setVolumeDelegate(value);
                        NotifyOfPropertyChange(() => Volume);
                    }
                }
            }
        }

        public override void Dispose()
        {
            Stop();

            lock (_playerLock)
            {
                if (_wavePlayer != null)
                {
                    _wavePlayer.Dispose();
                    _wavePlayer = null;
                }

                if (_inputStream != null)
                {
                    try
                    {
                        _inputStream.Dispose();
                    }

                        // TODO: NAudio sometimes thows an exception here for unknown reasons
                    catch (MmException)
                    {
                    }

                    _inputStream = null;
                }
            }
        }

        public override void Load(RawTrack song, float volume)
        {
            lock (_playerLock)
            {
                if (song == null)
                    Throw.ArgumentNullException(() => song);

                Song = song;
                Volume = volume;

                CreateWavePlayer();

                try
                {
                    ISampleProvider sampleProvider = CreateInputStream(Song.FullFilename);
                    _wavePlayer.Init(new SampleToWaveProvider(sampleProvider));
                }
                catch (Exception ex)
                {
                    throw new SongLoadException("Song could not be loaded.", ex);
                }

                _isLoaded = true;
            }
        }


        private void CreateWavePlayer()
        {
            CloseWavePlayer();
            int latency = Settings.Instance.RequestedLatency;
            try
            {
                _wavePlayer = Settings.Instance.SelectedOutputDriver.CreateDevice(latency);
            }
            catch (Exception driverCreateException)
            {
                MessageBox.Show(String.Format("{0}", driverCreateException.Message));
            }
        }

        private void CloseWavePlayer()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Stop();
            }
            if (_inputStream != null)
            {
                // this one really closes the file and ACM conversion
                _inputStream.Dispose();
                _setVolumeDelegate = null;
            }
            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }
        }

        public override void Pause()
        {
            lock (_playerLock)
            {
                if (_wavePlayer == null || _inputStream == null ||
                    _wavePlayer.PlaybackState == NAudio.Wave.PlaybackState.Paused
                    || _wavePlayer.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
                    return;

                _wavePlayer.Pause();

                EnsureState(AudioPlayerState.Paused);
            }
        }

        public override void Play()
        {
            lock (_playerLock)
            {
                if (_wavePlayer == null || _inputStream == null ||
                    _wavePlayer.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    return;

                // Create a new thread, so that we can spawn the song state check on the same thread as the play method
                // With this, we can avoid cross-threading issues with the NAudio library
                Task.Factory.StartNew(() =>
                    {
                        bool wasPaused = PlaybackState == AudioPlayerState.Paused;

                        try
                        {
                            _wavePlayer.Play();
                        }

                        catch (MmException ex)
                        {
                            throw new PlaybackException("The playback couldn't be started.", ex);
                        }

                        if (!wasPaused)
                        {
                            while (PlaybackState != AudioPlayerState.Stopped && PlaybackState != AudioPlayerState.None)
                            {
                                UpdateSongState();
                                Thread.Sleep(250);
                            }
                        }
                    });

                EnsureState(AudioPlayerState.Playing);
            }
        }

        public override void Stop()
        {
            lock (_playerLock)
            {
                if (_wavePlayer != null && _wavePlayer.PlaybackState != NAudio.Wave.PlaybackState.Stopped)
                {
                    _wavePlayer.Stop();
                    EnsureState(AudioPlayerState.Stopped);

                    _isLoaded = false;
                }
            }
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            IInputFileFormatPlugin plugin = GetPluginForFile(fileName);
            if (plugin == null)
            {
                throw new InvalidOperationException("Unsupported file extension");
            }
            _inputStream = plugin.CreateWaveStream(fileName);
            var waveChannel = new SampleChannel(_inputStream, true);
            _setVolumeDelegate = vol => waveChannel.Volume = vol;

            var postVolumeMeter = new MeteringSampleProvider(waveChannel);

            return postVolumeMeter;
        }

        private IInputFileFormatPlugin GetPluginForFile(string fileName)
        {
            return (InputFileFormats
                .Where(f => fileName.EndsWith(f.Extension, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();
        }


        private void EnsureState(AudioPlayerState state)
        {
            while (PlaybackState != state)
            {
                Thread.Sleep(200);
            }
        }

        private void UpdateSongState()
        {
            if (CurrentTime >= TotalTime)
            {
                Stop();
                OnSongFinished(EventArgs.Empty);
                return;
            }
        }
    }
}