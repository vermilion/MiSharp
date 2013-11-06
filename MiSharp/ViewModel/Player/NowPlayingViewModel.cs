using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Playlist;
using MiSharp.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class NowPlayingViewModel : IHandle<List<Track>>, IHandle<TrackState>
    {
        private readonly IEventAggregator _events;

        public NowPlayingViewModel()
        {
            NowPlayingPlaylist = new Playlist<TrackViewModel>();
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);

            RemoveSelectedCommand = new ReactiveCommand();
            RemoveSelectedCommand.Subscribe(param => NowPlayingPlaylist.RemoveAt(NowPlayingPlaylist.CurrentIndex));

            PlaySelectedCommand = new ReactiveCommand();
            PlaySelectedCommand.Subscribe(param => _events.Publish(NowPlayingPlaylist.CurrentEntry.Model));
        }

        public ReactiveCommand RemoveSelectedCommand { get; private set; }
        public ReactiveCommand PlaySelectedCommand { get; private set; }

        public Playlist<TrackViewModel> NowPlayingPlaylist { get; set; }

        #region IHandle

        //event from lib to add new items to list
        public void Handle(List<Track> songs)
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

        public Track GetNextSong(bool repeat, bool random)
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

        public Track GetPreviousSong(bool repeat, bool random)
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

        private Track GetRandom()
        {
            if (NowPlayingPlaylist.MoveRandom())
                return NowPlayingPlaylist.CurrentEntry.Model;
            return null;
        }
    }
}