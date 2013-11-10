using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Repository.FileStorage;
using Rareform.Collections;
using ReactiveUI;

namespace MiSharp
{
    public class AlbumViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;

        public AlbumViewModel(Album album)
        {
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();
            Model = album;
            Tracks = new ObservableList<TrackViewModel>();
            Tracks.AddRange(album.Tracks.Select(x => new TrackViewModel(x)));

            AddAlbumToPlaylistCommand = new ReactiveCommand();
            AddAlbumToPlaylistCommand.Subscribe(param => _events.Publish(Tracks.Select(x => x.Model).ToList()));

            EditorEditAlbumsCommand = new ReactiveCommand();
            EditorEditAlbumsCommand.Subscribe(
                param => _windowManager.ShowDialog(new AlbumTagEditorViewModel(Tracks.Select(x => x.Model.Model).ToList())));
        }

        public ReactiveCommand AddAlbumToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditAlbumsCommand { get; private set; }

        #region Properties

        public Album Model { get; set; }

        public ObservableList<TrackViewModel> Tracks { get; set; }

        public BitmapSource Cover
        {
            get { return IoC.Get<AlbumCoverRepository>().GetCover(Model.Title, Model.Artist.Name, Model.Identifier); }
        }

        #endregion
    }
}