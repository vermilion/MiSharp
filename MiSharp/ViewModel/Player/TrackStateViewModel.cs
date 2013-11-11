using System.ComponentModel.Composition;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class TrackStateViewModel : ReactiveObject
    {
        private AudioPlayerState _state;

        public TrackStateViewModel(Track track, AudioPlayerState state)
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