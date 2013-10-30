using DeadDog.Audio;
using MiSharp.Core.Player;

namespace MiSharp.ViewModel.Player
{
    public class TrackState
    {
        public TrackState(RawTrack track, AudioPlayerState state)
        {
            Track = track;
            State = state;
        }

        public RawTrack Track { get; private set; }
        public AudioPlayerState State { get; private set; }
    }
}