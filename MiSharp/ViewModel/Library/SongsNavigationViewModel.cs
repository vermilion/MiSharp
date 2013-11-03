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

        public SongsNavigationViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

        private ArtistViewModel SelectedBand { get; set; }
        private AlbumViewModel SelectedAlbum { get; set; }

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