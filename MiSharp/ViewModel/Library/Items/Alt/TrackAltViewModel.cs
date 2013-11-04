using System;
using System.Collections.Generic;
using Caliburn.Micro;
using DeadDog.Audio;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    public class TrackAltViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private AudioPlayerState _playingState;

        public TrackAltViewModel(RawTrack track)
        {
            Model = track;
            PlayingState = AudioPlayerState.None;

            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();

            AddSongToPlaylistCommand = new ReactiveCommand();
            AddSongToPlaylistCommand.Subscribe(param => _events.Publish(new List<RawTrack> {Model}));

            EditorEditSongsCommand = new ReactiveCommand();
            EditorEditSongsCommand.Subscribe(
                param => _windowManager.ShowDialog(new SongTagEditorViewModel(new List<RawTrack> {Model})));
        }

        public ReactiveCommand AddSongToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditSongsCommand { get; private set; }

        #region Properties

        public RawTrack Model { get; set; }

        public AudioPlayerState PlayingState
        {
            get { return _playingState; }
            set { this.RaiseAndSetIfChanged(ref _playingState, value); }
        }

        #endregion
    }
}