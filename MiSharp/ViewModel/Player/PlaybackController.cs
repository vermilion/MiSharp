using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using MiSharp.ViewModel.Player.Panes;
using ReactiveUI;
using WPFSoundVisualizationLib;

namespace MiSharp.ViewModel.Player
{
    public class PlaybackController : ReactiveObject, IHandle<List<Track>>, ISpectrumPlayer, IDisposable
    {
        public readonly AudioPlayerEngine AudioPlayerEngine;
        private readonly ObservableAsPropertyHelper<bool> _isMuted;
        private TrackStateViewModel _currentTrack;
        private bool _isPlaying;
        private float _tempVolume;
        private float _volume = 1.0f;

        public PlaybackController(IEventAggregator events)
        {
            AudioPlayerEngine = new AudioPlayerEngine();
            AudioPlayerEngine.SetNextFileAction(() => PlayNext());
            
            CurrentPlaylist = new TrackPlaylist();

            events.Subscribe(this);

            CurrentTrackChanged = this.WhenAnyValue(x => x.CurrentTrack.Track);
            VolumeChanged = this.WhenAnyValue(x => x.Volume);
            IsMutedChanged = this.WhenAnyValue(x => x.IsMuted);

            _isMuted = this.WhenAnyValue(x => x.Volume, x => Equals(x, 0.0f)).ToProperty(this, x => x.IsMuted);
        }


        public IObservable<Track> CurrentTrackChanged { get; set; }
        public IObservable<float> VolumeChanged { get; set; }
        public IObservable<bool> IsMutedChanged { get; set; }

        #region Properties

        public TrackStateViewModel CurrentTrack
        {
            get { return _currentTrack; }
            set { this.RaiseAndSetIfChanged(ref _currentTrack, value); }
        }

        public TrackPlaylist CurrentPlaylist { get; set; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { this.RaiseAndSetIfChanged(ref _isPlaying, value); }
        }


        public bool IsMuted
        {
            get { return _isMuted.Value; }
            set
            {
                if (value)
                {
                    _tempVolume = Volume;
                    Volume = 0;
                }
                else Volume = _tempVolume;
            }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                AudioPlayerEngine.Volume = value;
            }
        }

        public bool RepeatState
        {
            get { return Core.SettingsModel.Instance.RepeatState; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.RepeatState, value); }
        }

        public bool ShuffleState
        {
            get { return Core.SettingsModel.Instance.ShuffleState; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.ShuffleState, value); }
        }

        public EqualizerEngine EqualizerEngine { get { return AudioPlayerEngine.EqualizerEngine; } }

        #endregion

        public async Task Play(TrackStateViewModel song)
        {
            if (CurrentTrack == null || !Equals(CurrentTrack, song))
            {
                if (CurrentPlaylist.MoveToEntry(song))
                {
                    CurrentTrack = CurrentPlaylist.CurrentEntry;
                    await Task.Run(() =>
                        {
                            AudioPlayerEngine.Stop();
                            AudioPlayerEngine.Load(CurrentPlaylist.CurrentEntry.Track.Model);
                            AudioPlayerEngine.Play();
                        });
                }
            }
            else AudioPlayerEngine.Resume();
            IsPlaying = true;
            await CurrentPlaylist.SetState(song, AudioPlayerState.Playing);
        }

        public async Task PlayStream(string url)
        {
            await Task.Run(() =>
                {
                    AudioPlayerEngine.Stop();
                    AudioPlayerEngine.Load(url);
                    AudioPlayerEngine.Play();
                });

            IsPlaying = true;
        }

        public async Task PlayNext()
        {
            IsPlaying = false;
            AudioPlayerEngine.Stop();
            await CurrentPlaylist.SetState(CurrentTrack, AudioPlayerState.None);
            CurrentTrack = null;
            TrackStateViewModel song = await Task.Run(() => GetNextSong(RepeatState, ShuffleState));

            if (song != null)
                await Play(song);
        }

        public async Task PlayPrev()
        {
            IsPlaying = false;
            AudioPlayerEngine.Stop();
            await CurrentPlaylist.SetState(CurrentTrack, AudioPlayerState.None);
            CurrentTrack = null;
            TrackStateViewModel song = await Task.Run(() => GetPreviousSong(RepeatState, ShuffleState));
            if (song != null)
                await Play(song);
        }

        public async Task PlayPause()
        {
            if (IsPlaying)
            {
                await Task.Run(() => AudioPlayerEngine.Pause());
                IsPlaying = false;
                await CurrentPlaylist.SetState(CurrentTrack.Track, AudioPlayerState.Paused);
            }
            else
            {
                TrackStateViewModel song;
                if (CurrentPlaylist.CurrentEntry != null)
                    song = CurrentPlaylist.CurrentEntry;
                else if (CurrentPlaylist.Count > 0)
                    song = CurrentPlaylist[0];
                else return;

                await Play(song);
            }
        }

        public TrackStateViewModel GetNextSong(bool repeat, bool random)
        {
            if (random) return GetRandom();
            if (!repeat)
            {
                if (CurrentPlaylist.MoveNext())
                    return CurrentPlaylist.CurrentEntry;
            }
            else if (CurrentPlaylist.MoveNextOrFirst())
                return CurrentPlaylist.CurrentEntry;

            return null;
        }

        public TrackStateViewModel GetPreviousSong(bool repeat, bool random)
        {
            if (random) return GetRandom();
            if (!repeat)
            {
                if (CurrentPlaylist.MovePrevious())
                    return CurrentPlaylist.CurrentEntry;
            }
            else if (CurrentPlaylist.MovePreviousOrLast())
                return CurrentPlaylist.CurrentEntry;

            return null;
        }

        private TrackStateViewModel GetRandom()
        {
            if (CurrentPlaylist.MoveRandom())
                return CurrentPlaylist.CurrentEntry;
            return null;
        }

        #region IHandle

        public void Handle(List<Track> message)
        {
            CurrentPlaylist.AddRange(message);
        }

        #endregion

        #region ISpectrumPlayer implementation

        public bool GetFFTData(float[] fftDataBuffer)
        {
            AudioPlayerEngine.GetSpectrum(fftDataBuffer);
            return true;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            const int sampleFrequency = 44100;
            const int maxFFT = 2048;

            return FFTFrequency2Index(frequency, maxFFT, sampleFrequency);
        }

        public static int FFTFrequency2Index(double frequency, double length, double samplerate)
        {
            var num = (int) Math.Round(length*frequency/samplerate);
            //Note that for a real input signal (imaginary parts all zero) the second half of the FFT (bins from N / 2 + 1 to N - 1)
            //contain no useful additional information (they have complex conjugate symmetry with the first N / 2 - 1 bins). 
            if (num > length/2 - 1)
                num = (int) (length/2 - 1);
            return num;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            AudioPlayerEngine.Dispose();
        }

        #endregion
    }
}