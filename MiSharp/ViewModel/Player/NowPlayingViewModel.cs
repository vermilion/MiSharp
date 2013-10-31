using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Playlist;
using MiSharp.Player;

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

        //event from lib to add new items to list
        public void Handle(List<RawTrack> songs)
        {
            NowPlayingPlaylist.AddRange(songs.Select(x => new TrackViewModel(x)));
        }

        //event from player to set track state
        public void Handle(TrackState message)
        {
            TrackViewModel track = NowPlayingPlaylist.FirstOrDefault(x => Equals(x.Model, message.Track));
            if (track == null) return;
            track.PlayingState = message.State;
        }

        #endregion

        public void PlaySelected()
        {
            _events.Publish(NowPlayingPlaylist.CurrentEntry.Model);
        }

        public RawTrack GetNextSong(bool repeat, bool random)
        {
            if (random) return GetRandom();
            if (!repeat)
            {
                if (NowPlayingPlaylist.MoveNext())
                    return NowPlayingPlaylist.CurrentEntry.Model;
            }
            else if (NowPlayingPlaylist.MoveNextOrFirst())
                return NowPlayingPlaylist.CurrentEntry.Model;

            return null;
        }

        public RawTrack GetPreviousSong(bool repeat, bool random)
        {
            if (random) return GetRandom();
            if (!repeat)
            {
                if (NowPlayingPlaylist.MovePrevious())
                    return NowPlayingPlaylist.CurrentEntry.Model;
            }
            else if (NowPlayingPlaylist.MovePreviousOrLast())
                return NowPlayingPlaylist.CurrentEntry.Model;

            return null;
        }

        private RawTrack GetRandom()
        {
            if (NowPlayingPlaylist.MoveRandom())
                return NowPlayingPlaylist.CurrentEntry.Model;
            return null;
        }
    }
}