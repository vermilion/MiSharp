using DeadDog.Audio.Libraries.Collections;
using ReactiveUI;

namespace DeadDog.Audio.Libraries
{
    public class Artist : ReactiveObject
    {
        #region Properties

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

        public AlbumCollection Albums { get; set; }

        #endregion

        public Artist(string name)
        {
            _isunknown = name == null;
            Albums = new AlbumCollection();

            _name = name ?? string.Empty;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}