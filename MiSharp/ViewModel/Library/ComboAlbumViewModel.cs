using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Libraries;
using TagLib;
using File = TagLib.File;

namespace MiSharp
{
    public class ComboAlbumViewModel : Album, INotifyPropertyChanged
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private BitmapSource _cover;
        private Track _selectedSong;

        public ComboAlbumViewModel(Album album)
            : base(album.Title, album.Year)
        {
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();
            Tracks = album.Tracks;
            _cover = new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/Music.ico"));
        }

        #region Properties

        public BitmapSource Cover
        {
            get
            {
                Track item = Tracks.FirstOrDefault();
                if (item != null)
                {
                    File file = File.Create(item.Model.FullFilename);
                    if (file.Tag.Pictures.Any())
                    {
                        IPicture pic = file.Tag.Pictures[0];
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
            set
            {
                _cover = value;
                OnPropertyChanged();
            }
        }


        //TODO: multiple
        public ObservableCollection<Track> SelectedSongs { get; set; }

        public Track SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddAlbumToPlaylist()
        {
            _events.Publish(Tracks.Select(x => x.Model).ToList());
        }

        public void AddSongToPlaylist()
        {
            _events.Publish(new List<RawTrack> {SelectedSong.Model});
        }

        public void EditorEditAlbumsNew()
        {
            _windowManager.ShowDialog(new AlbumTagEditorViewModel(Tracks.Select(x => x.Model).ToList()));
        }

        public void EditorEditSongs()
        {
            if (SelectedSong != null)
                _windowManager.ShowDialog(new SongTagEditorViewModel(new List<RawTrack> {SelectedSong.Model}));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}