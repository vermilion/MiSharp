using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Playlist;

namespace MiSharp
{
    [Export]
    public class PlaylistViewModel : IHandle<List<RawTrack>>
    {
        private readonly IEventAggregator _events;

        public PlaylistCollection<RawTrack> PlaylistCollection { get; set; }

        public Playlist<RawTrack> SelectedPlaylist { get; set; }

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            //TODO: remove it
            var playlist = new Playlist<RawTrack>();
            playlist.Name = "Now playing";
            PlaylistCollection = new PlaylistCollection<RawTrack>();
            PlaylistCollection.Add(playlist);
            var playlist2 = new Playlist<RawTrack>();
            playlist2.Name = "Test";
            PlaylistCollection.Add(playlist2);
            SelectedPlaylist = playlist;
            _events = events;
            events.Subscribe(this);
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
            SelectedPlaylist.AddRange(songs);
        }

        public void PlaySelected()
        {
            _events.Publish(SelectedPlaylist.CurrentEntry);
        }

        #endregion

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