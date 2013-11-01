using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core.CustomEventArgs;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (LibraryViewModel))]
    public class LibraryViewModel : ReactiveObject, IHandle<FileStatEventArgs>, IHandle<ScanCompletedEventArgs>
    {
        private readonly IEventAggregator _events;
        private IEnumerable<AlbumViewModel> _comboAlbums;
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

        public IEnumerable<AlbumViewModel> ComboAlbums
        {
            get
            {
                if (SelectedBand == null || SelectedBand.Albums.Count == 0) return new List<AlbumViewModel>();
                return SelectedBand.Albums.Select(x => new AlbumViewModel(x));
            }
            set { this.RaiseAndSetIfChanged(ref _comboAlbums, value); }
        }

        public IEnumerable<ArtistViewModel> Bands
        {
            get { return MediaRepository.Instance.GetLibrary().Artists.Select(x => new ArtistViewModel(x)); }
        }

        #region IHandle

        private bool _updatingWindowVisible;

        public bool UpdatingWindowVisible
        {
            get { return _updatingWindowVisible; }
            set { this.RaiseAndSetIfChanged(ref _updatingWindowVisible, value); }
        }

        public void Handle(FileStatEventArgs e)
        {
            UpdatingWindowVisible = true;
            Status = e.CurrentFileNumber + ":" + e.TotalFiles;
            this.RaisePropertyChanged("Status");
            if (e.Completed)
                this.RaisePropertyChanged("Bands");
        }

        public void Handle(ScanCompletedEventArgs message)
        {
            UpdatingWindowVisible = false;
            this.RaisePropertyChanged("Bands");
        }

        #endregion
    }
}