using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.ViewModel.Library.Items;

namespace MiSharp.ViewModel.Library
{
    [Export]
    public class AlbumNavigationViewModel : Screen, IViewModelParams<AlbumNavigationViewModel.DefaultNavigationArgs>
    {
        private readonly IEventAggregator _events;
        private ArtistViewModel _selectedBand;

        public AlbumNavigationViewModel()
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

        public AlbumViewModel SelectedAlbum { get; set; }

        public IEnumerable<AlbumViewModel> ComboAlbums
        {
            get { return SelectedBand.Albums.Select(x => new AlbumViewModel(x)); }
        }

        public void ProcessParameters(DefaultNavigationArgs modelParams)
        {
            SelectedBand = modelParams.Value;
            NotifyOfPropertyChange(() => ComboAlbums);
        }

        public void ActivateAlbumSongs()
        {
            IoC.Get<INavigationService>()
               .Navigate(typeof (SongsNavigationViewModel),
                         new SongsNavigationViewModel.DefaultNavigationArgs(SelectedBand, SelectedAlbum));
        }

        //temp
        public void ToArtists()
        {
            IoC.Get<INavigationService>().Navigate(typeof (ArtistNavigationViewModel), null);
        }

        public class DefaultNavigationArgs
        {
            public DefaultNavigationArgs(ArtistViewModel value)
            {
                Value = value;
            }

            public ArtistViewModel Value { get; private set; }
        }
    }
}