using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core.Repository;

namespace MiSharp
{
    [Export]
    public class ArtistNavigationViewModel : Screen
    {
        private readonly IEventAggregator _events;

        public ArtistNavigationViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

        public ArtistViewModel SelectedBand { get; set; }

        public IEnumerable<ArtistViewModel> Bands
        {
            get { return MediaRepository.Instance.GetLibrary().Artists.Select(x => new ArtistViewModel(x)); }
        }

        public void ActivateAlbum()
        {
            IoC.Get<INavigationService>()
                .Navigate(typeof (AlbumNavigationViewModel),
                    new AlbumNavigationViewModel.DefaultNavigationArgs(SelectedBand));
        }
    }
}