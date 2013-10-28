using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio;
using Rareform.Collections;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlaylistViewModel : ReactiveObject, IHandle<List<RawTrack>>
    {
        private readonly IEventAggregator _events;
        private RawTrack _currentSong;

        private int _currentSongIndex;
        private ObservableList<RawTrack> _songs;

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            _events = events;
            _songs = new ObservableList<RawTrack>();
            events.Subscribe(this);
        }

        public ObservableList<RawTrack> Songs
        {
            get { return _songs; }
            set { this.RaiseAndSetIfChanged(ref _songs, value); }
        }

        public int CurrentSongIndex
        {
            get { return _currentSongIndex; }
            set { this.RaiseAndSetIfChanged(ref _currentSongIndex, value); }
        }

        public RawTrack CurrentSong
        {
            get { return _currentSong; }
            set { this.RaiseAndSetIfChanged(ref _currentSong, value); }
        }

        #region Triggers

        private bool CanPlayNextSong
        {
            get { return CurrentSongIndex + 1 < Songs.Count; }
        }

        private bool CanPlayPreviousSong
        {
            get
            {
                int prevIndex = CurrentSongIndex - 1;
                return prevIndex <= Songs.Count && prevIndex >= 0;
            }
        }

        public void RemoveSelected()
        {
            Songs.RemoveAt(CurrentSongIndex);
        }

        #endregion

        #region IHandle

        public void Handle(List<RawTrack> songs)
        {
            Songs.AddRange(songs);
        }

        public void PlaySelected()
        {
            _events.Publish(CurrentSong);
        }

        #endregion

        public RawTrack GetNextSong()
        {
            if (CanPlayNextSong)
            {
                RawTrack nextSong = Songs[CurrentSongIndex + 1];
                CurrentSongIndex++;
                return nextSong;
            }
            return null;
        }

        public RawTrack GetPreviousSong()
        {
            if (CanPlayPreviousSong)
            {
                RawTrack prevSong = Songs[CurrentSongIndex - 1];
                CurrentSongIndex--;
                return prevSong;
            }
            return null;
        }
    }
}