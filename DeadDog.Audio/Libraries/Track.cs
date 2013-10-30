using System.IO;

namespace DeadDog.Audio.Libraries
{
    public class Track
    {
        #region Fields and properties

        private readonly FileInfo _file;

        public bool FileExist
        {
            get
            {
                _file.Refresh();
                return _file.Exists;
            }
        }

        public string FilePath
        {
            get { return _file.FullName; }
        }

        public string Title { get; set; }

        public int? Tracknumber { get; set; }

        public Album Album { get; internal set; }

        public Artist Artist { get; internal set; }

        public RawTrack Model { get; set; }

        #endregion

        public Track(RawTrack trackinfo, Album album, Artist artist)
        {
            _file = trackinfo.File;
            Album = album;
            Artist = artist;
            Title = trackinfo.TrackTitle;
            Tracknumber = trackinfo.TrackNumberUnknown ? (int?) null : trackinfo.TrackNumber;
            Model = trackinfo;
        }

        public override string ToString()
        {
            return (Tracknumber == null ? "" : "#" + Tracknumber + " ") + Title;
        }
    }
}