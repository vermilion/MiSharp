using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using Caliburn.Micro;
using Rareform.Collections;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class SongsNavigationViewModel : Screen, IViewModelParams<SongsNavigationViewModel.DefaultNavigationArgs>
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;

        private AlbumViewModel _selectedAlbum;
        private ArtistViewModel _selectedBand;

        public SongsNavigationViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);

            _windowManager = IoC.Get<IWindowManager>();

            SelectionChangedCommand = new ReactiveCommand();
            SelectionChangedCommand.Where(x => x != null)
                .Select(x => ((IEnumerable)x).Cast<TrackViewModel>())
                .Subscribe(x => SelectedItems = x);

            AddSongsToPlaylistCommand = new ReactiveCommand();
            AddSongsToPlaylistCommand.Subscribe(param => 
                _events.Publish(SelectedItems.Select(x => x.Model).ToList()));

            EditorEditSongsCommand = new ReactiveCommand();
            EditorEditSongsCommand.Subscribe(param =>
                _windowManager.ShowDialog(new SongTagEditorViewModel(SelectedItems.Select(x => x.Model.Model).ToList())));
        }

        public ReactiveCommand SelectionChangedCommand { get; set; }
        public ReactiveCommand AddSongsToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditSongsCommand { get; private set; }

        public IEnumerable<TrackViewModel> SelectedItems { get; set; }    

        public ArtistViewModel SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                _selectedBand = value;
                NotifyOfPropertyChange(() => SelectedBand);
            }
        }

        public AlbumViewModel SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                NotifyOfPropertyChange(() => SelectedAlbum);
            }
        }

        public ObservableList<TrackViewModel> Tracks { get; set; }

        public void ProcessParameters(DefaultNavigationArgs modelParams)
        {
            SelectedBand = modelParams.Artist;
            SelectedAlbum = modelParams.Album;

            Tracks = new ObservableList<TrackViewModel>();
            Tracks.AddRange(SelectedAlbum.Tracks.Select(x => new TrackViewModel(x.Model)));

            NotifyOfPropertyChange(() => Tracks);
        }

        public void ToAlbums()
        {
            IoC.Get<INavigationService>()
               .Navigate(typeof (AlbumNavigationViewModel),
                         new AlbumNavigationViewModel.DefaultNavigationArgs(SelectedBand));
        }

        public class DefaultNavigationArgs
        {
            public DefaultNavigationArgs(ArtistViewModel artist, AlbumViewModel album)
            {
                Artist = artist;
                Album = album;
            }

            public ArtistViewModel Artist { get; private set; }
            public AlbumViewModel Album { get; private set; }
        }
    }
}