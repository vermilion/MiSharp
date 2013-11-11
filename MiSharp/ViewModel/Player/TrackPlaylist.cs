using System;
using System.Collections.Generic;
using System.Linq;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Playlist;
using MiSharp.Core.Player;

namespace MiSharp
{
    public class TrackPlaylist : Playlist<TrackStateViewModel>
    {
        public void Add(Track track)
        {
            Add(new TrackStateViewModel(track, AudioPlayerState.None));
        }

        public void AddRange(IEnumerable<Track> track)
        {
            track.ToList().ForEach(x => Add(new TrackStateViewModel(x, AudioPlayerState.None)));
        }

        public bool Contains(Track item)
        {
            return this.Select(x => x.Track).Contains(item);
        }

        public void SetState(Track track, AudioPlayerState state)
        {
            TrackStateViewModel item = this.First(x => x.Track.Equals(track));
            this.ToList().ForEach(x => x.State = AudioPlayerState.None);
            item.State = state;
        }

        public void SetState(TrackStateViewModel track, AudioPlayerState state)
        {
            if (track == null) return;
            TrackStateViewModel item = this.First(x => x.Equals(track));
            this.ToList().ForEach(x => x.State = AudioPlayerState.None);
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