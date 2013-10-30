using DeadDog.Audio;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    public class TrackViewModel : ReactiveObject
    {
        private AudioPlayerState _playingState;

        public TrackViewModel(RawTrack track)
        {
            Model = track;
            PlayingState = AudioPlayerState.None;
        }

        public RawTrack Model { get; set; }

        public AudioPlayerState PlayingState
        {
            get { return _playingState; }
            set { this.RaiseAndSetIfChanged(ref _playingState, value); }
        }
    }
}