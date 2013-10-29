using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Libraries;
using ReactiveUI;
using TagLib;
using File = TagLib.File;

namespace MiSharp
{
    public class ComboAlbumViewModel : Album
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private BitmapSource _cover;
        private Track _selectedSong;

        public ComboAlbumViewModel(Album album) : base(album.Title)
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
            set { this.RaiseAndSetIfChanged(ref _cover, value); }
        }


        //TODO: multiple
        public ObservableCollection<Track> SelectedSongs { get; set; }

        public Track SelectedSong
        {
            get { return _selectedSong; }
            set { this.RaiseAndSetIfChanged(ref _selectedSong, value); }
        }

        #endregion

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
            //_windowManager.ShowDialog(new AlbumTagEditorViewModel(Tracks));
        }

        public void EditorEditSongs()
        {
            // if (SelectedSong != null)
            //_windowManager.ShowDialog(new SongTagEditorViewModel(new List<Song> { SelectedSong }));
        }
    }
}