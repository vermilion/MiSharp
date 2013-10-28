using DeadDog.Audio.Libraries.Collections;
using ReactiveUI;

namespace DeadDog.Audio.Libraries
{
    public class Artist : ReactiveObject
    {
        #region Properties

        private AlbumCollection _albums;
        private readonly bool _isunknown;

        private string _name;

        public bool IsUnknown
        {
            get { return _isunknown; }
        }

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public AlbumCollection Albums
        {
            get { return _albums; }
            set { _albums = value; }
        }

        #endregion

        public Artist(string name)
        {
            _isunknown = name == null;
            _albums = new AlbumCollection();

            _name = name ?? string.Empty;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}