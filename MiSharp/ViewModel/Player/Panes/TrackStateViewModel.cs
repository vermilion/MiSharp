using System.ComponentModel.Composition;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using MiSharp.ViewModel.Library.Items;
using ReactiveUI;

namespace MiSharp.ViewModel.Player.Panes
{
    [Export]
    public class TrackStateViewModel : TrackViewModel
    {
        private AudioPlayerState _state;

        public TrackStateViewModel(Track track, AudioPlayerState state) : base(track)
        {
            State = state;
        }

        public AudioPlayerState State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }
    }
}