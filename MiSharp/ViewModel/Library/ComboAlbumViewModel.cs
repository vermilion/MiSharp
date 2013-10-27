using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.Library;
using MiSharp.Core.Repository;
using ReactiveUI;
using File = TagLib.File;

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

            _cover = new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/Music.ico"));
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
            get
            {
                var item = Songs.FirstOrDefault();
                if (item != null)
                {
                    File file = File.Create(item.OriginalPath);
                    if (file.Tag.Pictures.Any())
                    {
                        var pic = file.Tag.Pictures[0];
                        var ms = new MemoryStream(pic.Data.Data);
                        
                            ms.Seek(0, SeekOrigin.Begin);

                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                        
                        bitmap.EndInit();
                        _cover = bitmap;
                    }
                }
                return _cover;
            }
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

        public void EditorEditAlbumsNew()
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