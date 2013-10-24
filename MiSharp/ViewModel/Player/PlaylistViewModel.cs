using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.Player;
using Rareform.Collections;

namespace MiSharp
{
    [Export(typeof (PlaylistViewModel))]
    public class PlaylistViewModel : PropertyChangedBase, IHandle<List<Song>>
    {
        private readonly IEventAggregator _events;
        public LocalAudioPlayer Player;
        private int _currentSongIndex;
        private ObservableList<Song> _songs;

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            _events = events;
            Player = new LocalAudioPlayer();
            Player.PlaybackUpdated += Player_PlaybackUpdated;
            Player.SongFinished += Player_SongFinished;
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

        #region Triggers

        public void PlaySelected()
        {
            Player.Load(Songs[CurrentSongIndex]);
            Player.Play();
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

        #endregion

        #region Playback Events

        private void Player_SongFinished(object sender, EventArgs e)
        {
            int nextIndex = CurrentSongIndex + 1;
            if (nextIndex >= Songs.Count)
                return;
            CurrentSongIndex++;
            PlaySelected();
        }

        private void Player_PlaybackUpdated(PlaybackEventArgs args)
        {
            _events.Publish(args);
        }

        #endregion
    }
}