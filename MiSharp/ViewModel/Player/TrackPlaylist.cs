using System;
using System.Collections.Generic;
using System.Linq;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Playlist;
using MiSharp.Core.Player;
using Rareform.Extensions;

namespace MiSharp.ViewModel.Player
{
    public class TrackPlaylist : Playlist<TrackState>
    {
        public void Add(Track track)
        {
            Add(new TrackState(track, AudioPlayerState.None));
        }

        public void AddRange(IEnumerable<Track> track)
        {
            AddRange(track.Select(x => new TrackState(x, AudioPlayerState.None)));
        }

        public bool Contains(Track item)
        {
            return this.Select(x => x.Track).Contains(item);
        }

        public void SetState(Track track, AudioPlayerState state)
        {
            TrackState item = this.First(x => x.Track.Equals(track));
            this.ForEach(x => x.State = AudioPlayerState.None);
            item.State = state;
        }

        public void SetState(TrackState track, AudioPlayerState state)
        {
            if (track == null) return;
            TrackState item = this.First(x => x.Equals(track));
            this.ForEach(x => x.State = AudioPlayerState.None);
            item.State = state;
        }

        public bool Remove(Track item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(Track item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Track item)
        {
            throw new NotImplementedException();
        }

        public new IEnumerator<Track> GetEnumerator()
        {
            return this.Select(x => x.Track).GetEnumerator();
        }
    }
}