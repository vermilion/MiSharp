using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries
{
    public class Artist
    {
        #region Properties

        private readonly bool _isunknown;

        public bool IsUnknown
        {
            get { return _isunknown; }
        }

        public string Name { get; set; }

        public AlbumCollection Albums { get; set; }

        #endregion

        public Artist(string name)
        {
            _isunknown = name == null;
            Albums = new AlbumCollection();

            Name = name ?? "Unknown";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}