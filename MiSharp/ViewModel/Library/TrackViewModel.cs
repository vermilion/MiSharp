using System.Collections.Generic;
using Caliburn.Micro;
using DeadDog.Audio;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    public class TrackViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private AudioPlayerState _playingState;

        public TrackViewModel(RawTrack track)
        {
            Model = track;
            PlayingState = AudioPlayerState.None;

            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();
        }

        #region Properties

        public RawTrack Model { get; set; }

        public AudioPlayerState PlayingState
        {
            get { return _playingState; }
            set { this.RaiseAndSetIfChanged(ref _playingState, value); }
        }

        #endregion

        public void AddSongToPlaylist()
        {
            _events.Publish(new List<RawTrack> {Model});
        }

        public void EditorEditSongs()
        {
            _windowManager.ShowDialog(new SongTagEditorViewModel(new List<RawTrack> {Model}));
        }
    }
}