using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.EqualityComparers;
using MiSharp.Core.Library;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (LibraryViewModel))]
    public class LibraryViewModel : ReactiveObject, IHandle<FileStatEventArgs>
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private ArtistViewModel _selectedBand;
        private IEnumerable<ComboAlbumViewModel> _comboAlbums;

        [ImportingConstructor]
        public LibraryViewModel(IEventAggregator events, IWindowManager windowManager)
        {
            _events = events;
            _events.Subscribe(this);
            _windowManager = windowManager;
        }

        public string Status { get; set; }

        public ArtistViewModel SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedBand, value, "ComboAlbums");
            }
        }

        public IEnumerable<ComboAlbumViewModel> ComboAlbums
        {
            get
            {
                if (SelectedBand == null) return new List<ComboAlbumViewModel>();
                return MediaRepository.Instance
                    .GetAllSongs()
                    .Where(x => x.Artist == SelectedBand.Name)
                    .Distinct(new AlbumEqualityComparer())
                    .Select(x =>
                        new ComboAlbumViewModel
                        {
                            Name = x.Album,
                            Year = x.Year,
                            Artist = x.Artist
                        });
            }
            set { this.RaiseAndSetIfChanged(ref _comboAlbums, value); }
        }

        public IEnumerable<ArtistViewModel> Bands
        {
            get { return MediaRepository.Instance.GetAllBands().OrderBy(x => x).Select(x => new ArtistViewModel(x)); }
        }


        public void AddArtistToPlaylist()
        {
            _events.Publish(GetSongsByArtist());
        }
        
        private List<Song> GetSongsByArtist()
        {
            return MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(SelectedBand.Name, null)).ToList();
        }


        #region TagEditor
        
        public void EditorEditArtists()
        {
            if (SelectedBand != null)
                _windowManager.ShowDialog(new ArtistTagEditorViewModel(GetSongsByArtist()));
        }

        #endregion

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