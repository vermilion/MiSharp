using System;
using System.Linq;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using ReactiveUI;

namespace MiSharp
{
    public class ArtistViewModel : Artist
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;

        public ArtistViewModel(Artist artist)
            : base(artist.Name)
        {
            Albums = artist.Albums;
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();

            AddArtistToPlaylistCommand = new ReactiveCommand();
            AddArtistToPlaylistCommand.Subscribe(param => _events.Publish(Albums.SelectMany(x => x.Tracks).Select(x => x.Model).ToList()));

            EditorEditArtistsCommand = new ReactiveCommand();
            EditorEditArtistsCommand.Subscribe(param => _windowManager.ShowDialog(
                new ArtistTagEditorViewModel(Albums.SelectMany(x => x.Tracks).Select(x => x.Model).ToList())));
        }

        public ReactiveCommand AddArtistToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditArtistsCommand { get; private set; }

        #region Properties

        public int SongsCount
        {
            get { return Albums.Sum(x => x.Tracks.Count); }
        }

        public int AlbumsCount
        {
            get { return Albums.Count; }
        }

        #endregion
    }
}