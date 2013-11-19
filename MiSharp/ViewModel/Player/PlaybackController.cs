using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core;
using MiSharp.Core.Player;
using ReactiveUI;
using WPFSoundVisualizationLib;

namespace MiSharp
{
    public class PlaybackController : ReactiveObject, IHandle<List<Track>>, ISpectrumPlayer
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
            AudioPlayerEngine.SongFinished += (s, e) => PlayNext();

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
            get { return Settings.Instance.RepeatState; }
            set
            {
                this.RaiseAndSetIfChanged(ref Settings.Instance.RepeatState, value);
                Settings.Instance.SaveSettings();
            }
        }

        public bool ShuffleState
        {
            get { return Settings.Instance.ShuffleState; }
            set
            {
                this.RaiseAndSetIfChanged(ref Settings.Instance.ShuffleState, value);
                Settings.Instance.SaveSettings();
            }
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
            //TODO: stupid hack for array[2048] and freq [20,x]. Find better solution           
            if (frequency == 20)
                return 0;
            else return 2047;
        }

        #endregion
    }
}