using System.Linq;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;

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
        }

        public int SongsCount
        {
            get { return Albums.Sum(x => x.Tracks.Count); }
        }

        public int AlbumsCount
        {
            get { return Albums.Count; }
        }

        public void AddArtistToPlaylist()
        {
            _events.Publish(Albums.SelectMany(x => x.Tracks));
        }

        #region TagEditor

        public void EditorEditArtists()
        {
            //_windowManager.ShowDialog(new ArtistTagEditorViewModel(Albums.SelectMany(x => x.Tracks)));
        }

        #endregion
    }
}