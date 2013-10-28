using System;
using System.IO;

namespace DeadDog.Audio
{
    /// <summary>
    ///     Stores title, album, tracknumber and artist info for a track
    /// </summary>
    public class RawTrack : ICloneable, IEquatable<RawTrack>
    {
        /// <summary>
        ///     Gets the value tracknumber should equal if it is unknown (-1).
        /// </summary>
        public const int TrackNumberIfUnknown = -1;

        public const int YearIfUnknown = -1;
        private static readonly RawTrack unknown;
        private readonly int _year;

        private string _album, _artist;
        private FileInfo _file;
        private int _number;
        private string _track;

        static RawTrack()
        {
            unknown = new RawTrack();
        }

        /// <summary>
        ///     Should only be used in the static type constructor.
        /// </summary>
        private RawTrack()
        {
            _file = null;

            _track = null;
            _album = "Unknown";
            _number = TrackNumberIfUnknown;
            _year = YearIfUnknown;

            _artist = "Unknown";
        }

        /// <summary>
        ///     Initializes a new instance of the RawTrack class.
        /// </summary>
        /// <param name="filepath">The full path of the file.</param>
        /// <param name="tracktitle">The title of the track. Should be set to null if unknown.</param>
        /// <param name="albumtitle">The album name of the track. Should be set to null if unknown.</param>
        /// <param name="tracknumber">The tracknumber of the track on the album. Should be set to -1 if unknown.</param>
        /// <param name="artistname">The artistname for the track. Should be set to null if unknown.</param>
        public RawTrack(string filepath, string tracktitle, string albumtitle, int tracknumber, string artistname, int year)
        {
            if (filepath == null)
                throw new ArgumentNullException("filepath", "filepath cannot equal null");
            try
            {
                _file = new FileInfo(filepath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured from the passed filepath", "filepath", e);
            }

            _track = string.IsNullOrEmpty(tracktitle) ? null : tracktitle.Trim();
            _album = string.IsNullOrEmpty(albumtitle) ? null : albumtitle.Trim();
            _number = tracknumber;
            _artist = string.IsNullOrEmpty(artistname) ? null : artistname.Trim();
            _year = year;
        }

        internal static RawTrack Unknown
        {
            get { return unknown; }
        }

        internal bool IsUnknown
        {
            get { return ReferenceEquals(this, unknown); }
        }

        /// <summary>
        ///     Gets the title of the track. If the title is unknown, null is returned.
        /// </summary>
        public string TrackTitle
        {
            get { return _track; }
        }

        /// <summary>
        ///     Gets the album name of the track. If the title is unknown, null is returned.
        /// </summary>
        public string AlbumTitle
        {
            get { return _album; }
        }

        /// <summary>
        ///     Gets the tracknumber of the track on the album.
        /// </summary>
        public int TrackNumber
        {
            get { return _number; }
        }

        /// <summary>
        ///     Gets a boolean indicating whether the tracknumber is known.
        /// </summary>
        public bool TrackNumberUnknown
        {
            get { return _number == TrackNumberIfUnknown; }
        }

        /// <summary>
        ///     Gets the artistname for the track. If the artist is unknown, null is returned.
        /// </summary>
        public string ArtistName
        {
            get { return _artist; }
        }

        public int Year
        {
            get { return _year; }
        }

        public bool YearUnknown
        {
            get { return _number == YearIfUnknown; }
        }

        internal FileInfo File
        {
            get { return _file; }
        }

        /// <summary>
        ///     Gets the filename (e.g. mysong.mp3)
        /// </summary>
        public string Filename
        {
            get { return _file.Name; }
        }

        /// <summary>
        ///     Gets the full path of the file.
        /// </summary>
        public string FullFilename
        {
            get { return _file.FullName; }
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="RawTrack" /> that is a copy of the current instance.
        /// </summary>
        /// <returns>A new cloned instance of the current object.</returns>
        public object Clone()
        {
            var rt = new RawTrack();
            rt._album = _album;
            rt._artist = _artist;
            rt._file = _file;
            rt._number = _number;
            rt._track = _track;
            return rt;
        }

        internal static void SetNewArtistName(RawTrack item, string newname)
        {
            if (Equals(item, unknown))
                throw new ArgumentException("Cannot alter \"Unknown\"");

            item._artist = newname;
        }


        /// <summary>
        ///     Creates a human-readable string that represents this <see cref="RawTrack" />.
        /// </summary>
        /// <returns>
        ///     A string that represents this <see cref="RawTrack" />.
        /// </returns>
        public override string ToString()
        {
            if (_track != null)
            {
                if (_artist != null && _album != null)
                    return _artist + " [" + _album + "] - " + _track;
                else
                    return _track;
            }
            else
                return _file.FullName;
        }

        #region IEquatable<RawTrack> Members

        public bool Equals(RawTrack other)
        {
            return _file.FullName == other._file.FullName &&
                   _artist == other._artist &&
                   _album == other._album &&
                   _track == other._track &&
                   _number == other._number &&
                   _year == other._year;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is RawTrack)
                return Equals(obj as RawTrack);
            else
                return false;
        }

        #endregion
    }
}