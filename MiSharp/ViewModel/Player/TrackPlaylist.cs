using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Playlist;
using MiSharp.Core.Player;
using MiSharp.ViewModel.Player.Panes;

namespace MiSharp.ViewModel.Player
{
    public class TrackPlaylist : Playlist<TrackStateViewModel>
    {
        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1);

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

        public async Task SetState(Track track, AudioPlayerState state)
        {
            await _gate.WaitAsync();
            if (track == null) return;
            TrackStateViewModel item = this.First(x => x.Track.Equals(track));
            await Task.Run(() => Parallel.ForEach(this, x => x.State = AudioPlayerState.None));
            item.State = state;
            _gate.Release();
        }

        public async Task SetState(TrackStateViewModel track, AudioPlayerState state)
        {
            await _gate.WaitAsync();
            if (track == null) return;
            TrackStateViewModel item = this.First(x => x.Equals(track));
            await Task.Run(() => Parallel.ForEach(this, x => x.State = AudioPlayerState.None));
            item.State = state;
            _gate.Release();
        }

        public new IEnumerator<Track> GetEnumerator()
        {
            return this.Select(x => x.Track).GetEnumerator();
        }
    }
}