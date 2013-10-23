using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiSharp.Model;
using MiSharp.Model.Library;
using MiSharp.Model.Repository;

namespace MiSharp
{
    [Export(typeof (LibraryViewModel))]
    public class LibraryViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private Album _selectedAlbum;
        private string _selectedBand;

        [ImportingConstructor]
        public LibraryViewModel(IEventAggregator events, IWindowManager windowManager)
        {
            _events = events;
            _windowManager = windowManager;
        }

        public string Status { get; set; }

        public string SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                _selectedBand = value;
                NotifyOfPropertyChange(() => Albums);
            }
        }

        //TODO: multiple selection
        public Song SelectedSong { get; set; }


        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                NotifyOfPropertyChange(() => Songs);
            }
        }

        public IEnumerable<string> Bands
        {
            get { return MediaRepository.Instance.GetAllBands(); }
        }

        public IEnumerable<Album> Albums
        {
            get
            {
                if (SelectedBand == null) return new List<Album>();
                return MediaRepository.Instance.GetAllAlbums(SelectedBand);
            }
        }

        public IEnumerable<Song> Songs
        {
            get
            {
                if (SelectedBand == null || SelectedAlbum == null) return new List<Song>();
                return
                    MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(SelectedBand, SelectedAlbum.Name));
            }
        }

        public void AddToPlaylist()
        {
            _events.Publish(SelectedSong);
        }


        public void EditorEditAlbums()
        {
            if (SelectedAlbum == null) return;

            List<Song> list =
                MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(null, SelectedAlbum.Name)).ToList();
            _windowManager.ShowDialog(new AlbumTagEditorViewModel(list));
        }

        public void EditorEditArtists()
        {
            if (SelectedBand == null) return;

            List<Song> list = MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(SelectedBand, null)).ToList();
            _windowManager.ShowDialog(new ArtistTagEditorViewModel(list));
        }

        public void EditorEditSongs()
        {
            if (SelectedSong != null)
                _windowManager.ShowDialog(new SongTagEditorViewModel(new List<Song> {SelectedSong}));
        }

        public void RescanLibrary()
        {
            MediaRepository.Instance.Recreate();
            MediaRepository.Instance.ScanCompleted += Instance_ScanCompleted;
            MediaRepository.Instance.FileFound += Instance_FileFound;
            Task.Run(() => MediaRepository.Instance.Rescan());
        }

        private void Instance_FileFound(FileStatEventargs e)
        {
            Status = e.CurrentFileNumber + ":" + e.TotalFiles;
            NotifyOfPropertyChange(() => Status);
        }

        private void Instance_ScanCompleted()
        {
            NotifyOfPropertyChange(() => Bands);
        }
    }
}