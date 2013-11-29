using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core.Repository.Db4o;
using MiSharp.ViewModel.Library.Items;

namespace MiSharp.ViewModel.Library
{
    [Export]
    public class ArtistNavigationViewModel : Screen
    {
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