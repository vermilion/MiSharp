using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Playlist;
using MiSharp.ViewModel.Player;

namespace MiSharp
{
    [Export]
    public class NowPlayingViewModel : IHandle<List<RawTrack>>, IHandle<TrackState>
    {
        private readonly IEventAggregator _events;

        public NowPlayingViewModel()
        {
            NowPlayingPlaylist = new Playlist<TrackViewModel>();
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

        public Playlist<TrackViewModel> NowPlayingPlaylist { get; set; }

        public TrackViewModel SelectedTrack { get; set; }

        #region Triggers

        public void RemoveSelected()
        {
            NowPlayingPlaylist.RemoveAt(NowPlayingPlaylist.CurrentIndex);
        }

        #endregion

        #region IHandle

        public void Handle(List<RawTrack> songs)
        {
            NowPlayingPlaylist.AddRange(songs.Select(x => new TrackViewModel(x)));
        }

        public void PlaySelected()
        {
            _events.Publish(NowPlayingPlaylist.CurrentEntry.Model);
        }

        #endregion

        public void Handle(TrackState message)
        {
            TrackViewModel track = NowPlayingPlaylist.FirstOrDefault(x => x.Model == message.Track);
            if (track == null) return;
            track.PlayingState = message.State;
        }

        public RawTrack GetNextSong()
        {
            if (NowPlayingPlaylist.MoveNext())
                return NowPlayingPlaylist.CurrentEntry.Model;

            return null;
        }

        public RawTrack GetPreviousSong()
        {
            if (NowPlayingPlaylist.MovePrevious())
                return NowPlayingPlaylist.CurrentEntry.Model;

            return null;
        }
    }
}