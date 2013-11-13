using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    public class PlaybackController : ReactiveObject, IHandle<List<Track>>
    {
        public readonly LocalAudioPlayer AudioPlayer;
        private readonly ObservableAsPropertyHelper<bool> _isMuted;
        private TrackStateViewModel _currentTrack;
        private bool _isPlaying;
        private bool _repeatState;
        private bool _shuffleState;
        private float _tempVolume;
        private float _volume = 1.0f;

        public PlaybackController(IEventAggregator events)
        {
            AudioPlayer = new LocalAudioPlayer();
            AudioPlayer.SongFinished += (s, e) => PlayNext();

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
                AudioPlayer.Volume = value;
            }
        }

        public bool RepeatState
        {
            get { return _repeatState; }
            set { this.RaiseAndSetIfChanged(ref _repeatState, value); }
        }

        public bool ShuffleState
        {
            get { return _shuffleState; }
            set { this.RaiseAndSetIfChanged(ref _shuffleState, value); }
        }

        public void Play(TrackStateViewModel song)
        {
            if (CurrentTrack == null || !Equals(CurrentTrack, song))
            {
                if (CurrentPlaylist.MoveToEntry(song))
                {
                    CurrentTrack = CurrentPlaylist.CurrentEntry;
                    AudioPlayer.Load(CurrentPlaylist.CurrentEntry.Track.Model, Volume);
                }
            }
            AudioPlayer.Play();
            IsPlaying = true;
            CurrentPlaylist.SetState(song, AudioPlayerState.Playing);
        }

        public void PlayNext()
        {
            IsPlaying = false;
            AudioPlayer.Stop();
            CurrentPlaylist.SetState(CurrentTrack, AudioPlayerState.None);
            CurrentTrack = null;
            TrackStateViewModel song = GetNextSong(RepeatState, ShuffleState);

            if (song != null)
                Play(song);
        }

        public void PlayPrev()
        {
            IsPlaying = false;
            AudioPlayer.Stop();
            CurrentPlaylist.SetState(CurrentTrack, AudioPlayerState.None);
            CurrentTrack = null;
            TrackStateViewModel song = GetPreviousSong(RepeatState, ShuffleState);
            if (song != null)
                Play(song);
        }

        public void PlayPause()
        {
            if (IsPlaying)
            {
                AudioPlayer.Pause();
                IsPlaying = false;
               // CurrentPlaylist.SetState(CurrentTrack.Track, AudioPlayerState.Paused);
            }
            else
            {
                TrackStateViewModel song;
                if (CurrentPlaylist.CurrentEntry != null)
                    song = CurrentPlaylist.CurrentEntry;
                else if (CurrentPlaylist.Count > 0)
                    song = CurrentPlaylist[0];
                else return;

                Play(song);
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
    }
}