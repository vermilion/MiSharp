using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio;
using DeadDog.Audio.Libraries;
using Rareform.Collections;
using ReactiveUI;
using TagLib;
using File = TagLib.File;

namespace MiSharp
{
    public class AlbumViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private BitmapSource _cover;

        public AlbumViewModel(Album album)
        {
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();
            Model = album;
            Tracks = new ObservableList<TrackViewModel>();
            Tracks.AddRange(album.Tracks.Select(x => new TrackViewModel(x.Model)));
            _cover = new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/MusicAndCatalog.ico"));

            AddAlbumToPlaylistCommand = new ReactiveCommand();
            AddAlbumToPlaylistCommand.Subscribe(param => _events.Publish(Tracks.Select(x => x.Model).ToList()));

            EditorEditAlbumsCommand = new ReactiveCommand();
            EditorEditAlbumsCommand.Subscribe(param => _windowManager.ShowDialog(new AlbumTagEditorViewModel(Tracks.Select(x => x.Model).ToList())));
        }

        public ReactiveCommand AddAlbumToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditAlbumsCommand { get; private set; }

        #region Properties

        public BitmapSource Cover
        {
            get
            {
                RawTrack item = Tracks.Select(x => x.Model).FirstOrDefault();
                if (item != null)
                {
                    File file = File.Create(item.FullFilename);
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

        public Album Model { get; set; }

        public ObservableList<TrackViewModel> Tracks { get; set; }

        #endregion
    }
}