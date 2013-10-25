using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using Rareform.Collections;

namespace MiSharp
{
    [Export]
    public class PlaylistViewModel : PropertyChangedBase, IHandle<List<Song>>
    {
        private readonly IEventAggregator _events;
        private Song _currentSong;

        private int _currentSongIndex;
        private ObservableList<Song> _songs;

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            _events = events;
            _songs = new ObservableList<Song>();
            events.Subscribe(this);
        }

        public ObservableList<Song> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
                NotifyOfPropertyChange(() => Songs);
            }
        }

        public int CurrentSongIndex
        {
            get { return _currentSongIndex; }
            set
            {
                _currentSongIndex = value;
                NotifyOfPropertyChange(() => CurrentSongIndex);
            }
        }

        public Song CurrentSong
        {
            get { return _currentSong; }
            set
            {
                _currentSong = value;
                NotifyOfPropertyChange(() => CurrentSong);
            }
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

        public void Handle(List<Song> songs)
        {
            Songs.AddRange(songs);
        }

        public void PlaySelected()
        {
            _events.Publish(CurrentSong);
        }

        #endregion

        public Song GetNextSong()
        {
            if (CanPlayNextSong)
            {
                Song nextSong = Songs[CurrentSongIndex + 1];
                CurrentSongIndex++;
                return nextSong;
            }
            return null;
        }

        public Song GetPreviousSong()
        {
            if (CanPlayPreviousSong)
            {
                Song prevSong = Songs[CurrentSongIndex - 1];
                CurrentSongIndex--;
                return prevSong;
            }
            return null;
        }
    }
}