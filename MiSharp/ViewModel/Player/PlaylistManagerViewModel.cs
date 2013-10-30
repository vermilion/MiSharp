using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Playlist;

namespace MiSharp
{
    [Export]
    public class PlaylistManagerViewModel : IHandle<List<RawTrack>>
    {
        private readonly IEventAggregator _events;

        public PlaylistManagerViewModel()
        {
            NowPlayingPlaylist = new Playlist<RawTrack> {Name = "Now playing"};
            PlaylistCollection = new PlaylistCollection<RawTrack> {NowPlayingPlaylist};
            SelectedPlaylist = NowPlayingPlaylist;
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

        #region Triggers

        public void RemoveSelected()
        {
            SelectedPlaylist.RemoveAt(SelectedPlaylist.CurrentIndex);
        }

        #endregion

        #region IHandle

        public void Handle(List<RawTrack> songs)
        {
            //SelectedPlaylist.AddRange(songs);
        }

        public void PlaySelected()
        {
            _events.Publish(SelectedPlaylist.CurrentEntry);
        }

        #endregion

        public PlaylistCollection<RawTrack> PlaylistCollection { get; set; }

        public Playlist<RawTrack> SelectedPlaylist { get; set; }

        private Playlist<RawTrack> NowPlayingPlaylist { get; set; }

        public RawTrack GetNextSong()
        {
            if (SelectedPlaylist.MoveNext())
                return SelectedPlaylist.CurrentEntry;

            return null;
        }

        public RawTrack GetPreviousSong()
        {
            if (SelectedPlaylist.MovePrevious())
                return SelectedPlaylist.CurrentEntry;

            return null;
        }
    }
}