using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeadDog.Audio;
using Linsft.FmodSharp.Channel;
using Linsft.FmodSharp.Dsp;
using Linsft.FmodSharp.Enums;
using Linsft.FmodSharp.Error;
using Linsft.FmodSharp.Sound;
using Linsft.FmodSharp.SoundSystem;
using CallbackType = Linsft.FmodSharp.Channel.CallbackType;

namespace MiSharp.Core.Player
{
    public class AudioPlayerEngine : IDisposable
    {
        private readonly object _playerLock = new object();
        private SoundSystem _soundSystem;
        private Channel _channel;
        private bool _isLoaded;
        private Sound _soundFile;
        private float _volume = 1.0f;
        public EqualizerEngine EqualizerEngine { get; set; }

        private readonly ChannelDelegate _callback;

        private Action _playNextFileAction;

        public AudioPlayerEngine()
        {
            _soundSystem = new SoundSystem();
            _soundSystem.Init(32, InitFlags.Normal, (IntPtr) null);
            _callback = ChannelEndCallback;

            _soundSystem.SetStreamBufferSize(64 * 1024, TimeUnit.RawBytes);

            EqualizerEngine = new EqualizerEngine(_soundSystem);

            CurrentTimeChanged = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                                           .Select(x => CurrentTime)
                                           .DistinctUntilChanged(x => x.TotalSeconds);

            PlaybackStateChanged = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                             .Select(x => PlaybackState)
                                             .DistinctUntilChanged(x => x);

            TotalTimeChanged = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                                         .Select(x => TotalTime)
                                         .DistinctUntilChanged(x => x.TotalSeconds);
        }

        public IObservable<TimeSpan> CurrentTimeChanged { get; private set; }
        public IObservable<TimeSpan> TotalTimeChanged { get; private set; }
        public IObservable<AudioPlayerState> PlaybackStateChanged { get; private set; }

        public TimeSpan CurrentTime
        {
            get { return _channel == null ? TimeSpan.Zero : TimeSpan.FromMilliseconds(_channel.CurrentPositionMs); }
            set { if (_channel != null) _channel.CurrentPositionMs = Convert.ToUInt32(value.TotalMilliseconds); }
        }

        public TimeSpan TotalTime
        {
            get { return _isLoaded ? TimeSpan.FromMilliseconds(_soundFile.LengthMs) : TimeSpan.Zero; }
        }

        public AudioPlayerState PlaybackState
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

                return AudioPlayerState.Stopped;
            }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;

                if (_channel != null)
                {
                    _channel.Volume = value;
                }
            }
        }

        public void Load(RawTrack song)
        {
            lock (_playerLock)
            {
                _soundFile = _soundSystem.CreateSound(song.FullFilename);
                _isLoaded = true;
            }
        }

        public void Load(string url)
        {
            lock (_playerLock)
            {
                _soundFile = _soundSystem.CreateSound(url, Mode.CreateStream | Mode.NonBlocking);
                _isLoaded = true;
            }
        }

        public void Pause()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                    _channel.Paused = true;
            }
        }

        public void Resume()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                    _channel.Paused = false;
            }
        }

        public void Play()
        {
            Stop();
            Task.Factory.StartNew(() =>
                {
                    var openState = OpenState.Error;
                    uint percent = 0;

                    while (openState != OpenState.Ready)
                    {
                        _soundFile.GetOpenState(ref openState, ref percent);
                        Thread.Sleep(10);
                    }

                    _channel = _soundSystem.PlaySound(_soundFile);
                    _channel.Volume = Volume;
                    _channel.SetCallback(_callback);

                    while (_channel != null)
                    {
                        if (_soundSystem != null)
                            _soundSystem.Update();

                        Thread.Sleep(50);
                    }
                });
        }

        public void Stop()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                {
                    _channel.SetCallback(null);
                    _channel.Stop();
                    _channel = null;
                    _isLoaded = false;
                }
            }
        }

        public void SetNextFileAction(Action action)
        {
            _playNextFileAction = action;
        }

        private Code ChannelEndCallback(IntPtr channelraw, CallbackType type, IntPtr commanddata1, IntPtr commanddata2)
        {
            if (type == CallbackType.End)
            {
                Action action = _playNextFileAction;
                if (action != null)
                    action();
            }
            return Code.OK;
        }

        public void GetSpectrum(float[] spectrum)
        {
            int spectrumSize = spectrum.Length;

            int numChannels = 0;
            int dummy = 0;
            var dummyFormat = Format.None;
            var dummyResampler = Resampler.Linear;

            //TODO: investigate if we need following
            //we can get spectrum for all of our channels
            _soundSystem.GetSoftwareFormat(ref dummy, ref dummyFormat, ref numChannels, ref dummy, ref dummyResampler,
                                           ref dummy);

            for (int count = 0; count < numChannels; count++)
            {
                _soundSystem.GetSpectrum(spectrum, spectrumSize, count, FFTWindow.Triangle);
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Stop();

            if (EqualizerEngine != null)
            {
                EqualizerEngine.Dispose();
                EqualizerEngine = null;
            }
            if (_channel != null)
            {
                _soundFile.Dispose();
                _soundFile = null;
                _channel.Dispose();
                _channel = null;
            }

            if (_soundSystem != null)
            {
                _soundSystem.Dispose();
                _soundSystem = null;
            }
        }

        #endregion
    }
}