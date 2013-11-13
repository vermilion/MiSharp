using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeadDog.Audio;
using Linsft.FmodSharp.Channel;
using Linsft.FmodSharp.Reverb;
using Linsft.FmodSharp.Sound;
using Linsft.FmodSharp.SoundSystem;

namespace MiSharp.Core.Player
{
    public class LocalAudioPlayer : AudioPlayer
    {
        public delegate void PlaybackEventHandler(PlaybackEventArgs args);

        private readonly object _playerLock = new object();
        private readonly SoundSystem _soundSystem;
        private Channel _channel;
        private bool _isLoaded;
        private Action<float> _setVolumeDelegate;
        private Sound _soundFile;
        private float _volume;

        public LocalAudioPlayer()
        {
            _soundSystem = new SoundSystem();

            _soundSystem.Init();
            _soundSystem.ReverbProperties = Presets.Room;

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
            get { return _channel == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds(_channel.CurrentPositionMs); }
            set { if (_channel != null) _channel.CurrentPositionMs = Convert.ToUInt32(value.TotalMilliseconds); }
        }

        public override TimeSpan TotalTime
        {
            get { return _isLoaded ? TimeSpan.FromMilliseconds(_soundFile.LengthMs) : TimeSpan.Zero; }
        }

        public override AudioPlayerState PlaybackState
        {
            get
            {
                if (_channel != null)
                {
                    switch (_channel.PlaybackState)
                    {
                        case Channel.State.Stopped:
                            return AudioPlayerState.Stopped;
                        case Channel.State.Playing:
                            return AudioPlayerState.Playing;
                        case Channel.State.Paused:
                            return AudioPlayerState.Paused;
                    }
                }

                return AudioPlayerState.None;
            }
        }

        public override float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;

                if (_channel != null)
                {
                    if (_setVolumeDelegate != null)
                    {
                        _setVolumeDelegate(value);
                    }
                }
            }
        }

        public override void Dispose()
        {
            Stop();

            lock (_playerLock)
            {
                if (_channel != null)
                {
                    _soundFile.Dispose();
                    _channel.Dispose();
                }

                if (_soundSystem != null)
                {
                    _soundSystem.Dispose();
                }
            }
        }

        public override void Load(RawTrack song, float volume)
        {
            _soundFile = _soundSystem.CreateSound(song.FullFilename);
            //is that really enough?
            _isLoaded = true;
        }


        public override void Pause()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                    _channel.Paused = true;

                EnsureState(AudioPlayerState.Paused);
            }
        }

        public override void Play()
        {
            lock (_playerLock)
            {
                Task.Factory.StartNew(() =>
                    {
                        _channel = _soundSystem.PlaySound(_soundFile);
                        _setVolumeDelegate = vol => _channel.Volume = vol;

                        while (_channel.IsPlaying)
                        {
                            UpdateSongState();
                            Thread.Sleep(10);
                        }
                        EnsureState(AudioPlayerState.Playing);
                    });
            }
        }

        public override void Stop()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                {
                    _channel.Stop();
                    EnsureState(AudioPlayerState.Stopped);
                    _isLoaded = false;
                }
            }
        }

        private void EnsureState(AudioPlayerState state)
        {
            while (PlaybackState != state)
            {
                Thread.Sleep(50);
            }
        }

        private void UpdateSongState()
        {
            if (CurrentTime >= TotalTime)
            {
                Stop();
                OnSongFinished(EventArgs.Empty);
            }
        }
    }
}