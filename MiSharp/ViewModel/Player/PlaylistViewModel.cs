using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using Rareform.Collections;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlaylistViewModel : ReactiveObject, IHandle<IEnumerable<Song>>
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
            set { this.RaiseAndSetIfChanged(ref _songs, value); }
        }

        public int CurrentSongIndex
        {
            get { return _currentSongIndex; }
            set { this.RaiseAndSetIfChanged(ref _currentSongIndex, value); }
        }

        public Song CurrentSong
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

        public void Handle(IEnumerable<Song> songs)
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