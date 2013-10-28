using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core.Library;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (LibraryViewModel))]
    public class LibraryViewModel : ReactiveObject, IHandle<FileStatEventArgs>
    {
        private readonly IEventAggregator _events;
        private IEnumerable<ComboAlbumViewModel> _comboAlbums;
        private ArtistViewModel _selectedBand;

        public LibraryViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _events.Subscribe(this);
        }

        public string Status { get; set; }

        public ArtistViewModel SelectedBand
        {
            get { return _selectedBand; }
            set { this.RaiseAndSetIfChanged(ref _selectedBand, value, "ComboAlbums"); }
        }

        public IEnumerable<ComboAlbumViewModel> ComboAlbums
        {
            get
            {
                if (SelectedBand == null || SelectedBand.Albums.Count==0) return new List<ComboAlbumViewModel>();
                return SelectedBand.Albums
                                   //.Where(x => x.Artist.Name == SelectedBand.Name)
                                   .Select(x => new ComboAlbumViewModel(x));
            }
            set { this.RaiseAndSetIfChanged(ref _comboAlbums, value); }
        }

        public IEnumerable<ArtistViewModel> Bands
        {
            get { return MediaRepository.Instance.GetLibrary().Artists.Select(x => new ArtistViewModel(x)); }
        }

        #region IHandle

        public void Handle(FileStatEventArgs e)
        {
            Status = e.CurrentFileNumber + ":" + e.TotalFiles;
            raisePropertyChanged("Status");
            //NotifyOfPropertyChange(() => Status);
            if (e.Completed)
                raisePropertyChanged("Bands");
            //NotifyOfPropertyChange(() => Bands);
        }

        #endregion
    }
}