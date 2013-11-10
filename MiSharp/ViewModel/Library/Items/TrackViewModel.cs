using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    public class TrackViewModel : ReactiveObject
    {
        private AudioPlayerState _playingState;

        public TrackViewModel(Track track)
        {
            Model = track;
            PlayingState = AudioPlayerState.None;
        }

        #region Properties

        public Track Model { get; set; }

        public AudioPlayerState PlayingState
        {
            get { return _playingState; }
            set { this.RaiseAndSetIfChanged(ref _playingState, value); }
        }

        #endregion
    }
}