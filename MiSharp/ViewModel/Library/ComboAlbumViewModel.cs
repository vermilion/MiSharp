using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.Library;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    public class ComboAlbumViewModel : ReactiveObject
    {
        private string _name;
        private IEnumerable<Song> _songs;
        private string _year;
        private BitmapSource _cover;
        private string _artist;
        private Song _selectedSong;

        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;

        public ComboAlbumViewModel()
        {
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();
        }

        #region Properties

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public string Artist
        {
            get { return _artist; }
            set { this.RaiseAndSetIfChanged(ref _artist, value); }
        }

        public BitmapSource Cover
        {
            get { return _cover; }
            set { this.RaiseAndSetIfChanged(ref _cover, value); }
        }

        public string Year
        {
            get { return _year; }
            set { this.RaiseAndSetIfChanged(ref _year, value); }
        }

        //TODO: multiple
        public ObservableCollection<Song> SelectedSongs { get; set; }

        public Song SelectedSong
        {
            get { return _selectedSong; }
            set { this.RaiseAndSetIfChanged(ref _selectedSong, value); }
        }

        public IEnumerable<Song> Songs
        {
            get
            {
                if (Artist == null && Name == null) return new List<Song>();
                _songs = MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(Artist, Name));
                return _songs;
            }
            set { this.RaiseAndSetIfChanged(ref _songs, value); }
        }

        #endregion

        public void AddAlbumToPlaylist()
        {
            _events.Publish(Songs);
        }

        public void AddSongToPlaylist()
        {
            _events.Publish(new List<Song> { SelectedSong });
        }

        public void EditorEditAlbumsNew(string albumName)
        {
            _windowManager.ShowDialog(new AlbumTagEditorViewModel(Songs.ToList()));
        }

        public void EditorEditSongs()
        {
            if (SelectedSong != null)
                _windowManager.ShowDialog(new SongTagEditorViewModel(new List<Song> { SelectedSong }));
        }

    }
}