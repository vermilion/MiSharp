using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp.ViewModel.Player
{
    public class TrackState : ReactiveObject
    {
        private AudioPlayerState _state;

        public TrackState(Track track, AudioPlayerState state)
        {
            Track = track;
            State = state;
        }

        public Track Track { get; private set; }

        public AudioPlayerState State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }
    }
}