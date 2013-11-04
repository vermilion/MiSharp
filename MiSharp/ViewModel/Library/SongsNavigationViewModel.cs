using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Rareform.Collections;

namespace MiSharp
{
    [Export]
    public class SongsNavigationViewModel : Screen, IViewModelParams<SongsNavigationViewModel.DefaultNavigationArgs>
    {
        private readonly IEventAggregator _events;
        private AlbumViewModel _selectedAlbum;
        private ArtistViewModel _selectedBand;

        public SongsNavigationViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

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