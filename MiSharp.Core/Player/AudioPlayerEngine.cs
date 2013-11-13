using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DeadDog.Audio;
using Linsft.FmodSharp;
using Linsft.FmodSharp.Channel;
using Linsft.FmodSharp.Reverb;
using Linsft.FmodSharp.Sound;
using Linsft.FmodSharp.SoundSystem;
using Rareform.Extensions;

namespace MiSharp.Core.Player
{
    public class AudioPlayerEngine
    {
        public delegate void PlaybackEventHandler(PlaybackEventArgs args);

        private readonly object _playerLock = new object();
        private readonly SoundSystem _soundSystem;
        private Channel _channel;
        private bool _isLoaded;
        private Sound _soundFile;
        private float _volume;

        public AudioPlayerEngine()
        {
            _soundSystem = new SoundSystem();

            _soundSystem.Init();
            _soundSystem.ReverbProperties = Presets.Room;

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

        public void Dispose()
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

        public void Load(RawTrack song, float volume)
        {
            lock (_playerLock)
            {
                _soundFile = _soundSystem.CreateSound(song.FullFilename);
                //is that really enough?
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
            lock (_playerLock)
            {
                Task.Factory.StartNew(() =>
                    {
                        _channel = _soundSystem.PlaySound(_soundFile);

                        while (_channel != null && _channel.IsPlaying)
                        {
                            UpdateSongState();
                            Thread.Sleep(10);
                        }
                    });
            }
        }

        public void Stop()
        {
            lock (_playerLock)
            {
                if (_channel != null)
                {
                    _channel.Stop();
                    _channel = null;
                    _isLoaded = false;
                }
            }
        }


        private void UpdateSongState()
        {
            //TODO: temp OnNext fix. Get rid of End callback or find another way
            if (CurrentTime < TotalTime.Add(new TimeSpan(0,0,0,0,-50)))
                return;
            
            Stop();
            OnSongFinished(EventArgs.Empty);
        }

        public event EventHandler SongFinished;

        protected void OnSongFinished(EventArgs e)
        {
            SongFinished.RaiseSafe(this, e);
        }

    }
}