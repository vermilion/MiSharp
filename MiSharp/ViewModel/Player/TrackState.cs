using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;

namespace MiSharp.Player
{
    public class TrackState
    {
        public TrackState(Track track, AudioPlayerState state)
        {
            Track = track;
            State = state;
        }

        public Track Track { get; private set; }
        public AudioPlayerState State { get; private set; }
    }
}