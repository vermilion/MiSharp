using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using Caliburn.Micro;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class NowPlayingViewModel : Screen
    {
        private readonly PlaybackController _playbackController;

        public NowPlayingViewModel()
        {
            _playbackController = IoC.Get<PlaybackController>();

            SelectionChangedCommand = new ReactiveCommand();
            SelectionChangedCommand.Where(x => x != null)
                .Select(x => ((IEnumerable) x).Cast<TrackStateViewModel>())
                .Subscribe(x => SelectedItems = x);

            RemoveSelectedCommand = new ReactiveCommand();
            RemoveSelectedCommand.Subscribe(param =>
                new List<TrackStateViewModel>(SelectedItems)
                    .ForEach(x => Playlist.Remove(x)));

            RemoveAllCommand = new ReactiveCommand();
            RemoveAllCommand.Subscribe(param => Playlist.Clear());

            PlaySelectedCommand = new ReactiveCommand();
            PlaySelectedCommand.Subscribe(param => _playbackController.Play(SelectedItems.First()));
        }

        public ReactiveCommand RemoveSelectedCommand { get; private set; }
        public ReactiveCommand RemoveAllCommand { get; private set; }
        public ReactiveCommand PlaySelectedCommand { get; private set; }
        public ReactiveCommand SelectionChangedCommand { get; set; }

        public TrackPlaylist Playlist
        {
            get { return _playbackController.CurrentPlaylist; }
        }


        public IEnumerable<TrackStateViewModel> SelectedItems { get; set; }        
    }
}