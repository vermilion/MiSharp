using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.ViewModel.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class NowPlayingViewModel
    {
        private readonly PlaybackController _playbackController;

        public NowPlayingViewModel()
        {
            _playbackController = IoC.Get<PlaybackController>();

            RemoveSelectedCommand = new ReactiveCommand();
            RemoveSelectedCommand.Subscribe(param => { if (SelectedItem != null) Playlist.RemoveAt(SelectedIndex); });

            PlaySelectedCommand = new ReactiveCommand();
            PlaySelectedCommand.Subscribe(param => _playbackController.Play(SelectedItem));
        }

        public ReactiveCommand RemoveSelectedCommand { get; private set; }
        public ReactiveCommand PlaySelectedCommand { get; private set; }

        public TrackPlaylist Playlist
        {
            get { return _playbackController.CurrentPlaylist; }
        }


        public TrackState SelectedItem { get; set; }
        public int SelectedIndex { get; set; }
    }
}