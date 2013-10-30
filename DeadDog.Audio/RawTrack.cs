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

        static RawTrack()
        {
            unknown = new RawTrack();
        }

        /// <summary>
        ///     Should only be used in the static type constructor.
        /// </summary>
        protected RawTrack()
        {
            File = null;

            TrackTitle = null;
            AlbumTitle = "Unknown";
            TrackNumber = TrackNumberIfUnknown;
            Year = YearIfUnknown;

            ArtistName = "Unknown";
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
                File = new FileInfo(filepath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured from the passed filepath", "filepath", e);
            }

            TrackTitle = string.IsNullOrEmpty(tracktitle) ? null : tracktitle.Trim();
            AlbumTitle = string.IsNullOrEmpty(albumtitle) ? null : albumtitle.Trim();
            TrackNumber = tracknumber;
            ArtistName = string.IsNullOrEmpty(artistname) ? null : artistname.Trim();
            Year = year;
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
        public string TrackTitle { get; set; }

        /// <summary>
        ///     Gets the album name of the track. If the title is unknown, null is returned.
        /// </summary>
        public string AlbumTitle { get; set; }

        /// <summary>
        ///     Gets the tracknumber of the track on the album.
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        ///     Gets a boolean indicating whether the tracknumber is known.
        /// </summary>
        public bool TrackNumberUnknown
        {
            get { return TrackNumber == TrackNumberIfUnknown; }
        }

        /// <summary>
        ///     Gets the artistname for the track. If the artist is unknown, null is returned.
        /// </summary>
        public string ArtistName { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public uint Bitrate { get; set; }

        public TimeSpan Duration { get; set; }

        public bool YearUnknown
        {
            get { return Year == YearIfUnknown; }
        }

        internal FileInfo File { get; set; }

        /// <summary>
        ///     Gets the filename (e.g. mysong.mp3)
        /// </summary>
        public string Filename
        {
            get { return File.Name; }
        }

        /// <summary>
        ///     Gets the full path of the file.
        /// </summary>
        public string FullFilename
        {
            get { return File.FullName; }
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="RawTrack" /> that is a copy of the current instance.
        /// </summary>
        /// <returns>A new cloned instance of the current object.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        internal static void SetNewArtistName(RawTrack item, string newname)
        {
            if (Equals(item, unknown))
                throw new ArgumentException("Cannot alter \"Unknown\"");

            item.ArtistName = newname;
        }


        /// <summary>
        ///     Creates a human-readable string that represents this <see cref="RawTrack" />.
        /// </summary>
        /// <returns>
        ///     A string that represents this <see cref="RawTrack" />.
        /// </returns>
        public override string ToString()
        {
            if (TrackTitle != null)
            {
                if (ArtistName != null && AlbumTitle != null)
                    return ArtistName + " [" + AlbumTitle + "] - " + TrackTitle;
                else
                    return TrackTitle;
            }
            else
                return File.FullName;
        }

        #region IEquatable<RawTrack> Members

        public bool Equals(RawTrack other)
        {
            return File.FullName == other.File.FullName &&
                   ArtistName == other.ArtistName &&
                   AlbumTitle == other.AlbumTitle &&
                   TrackTitle == other.TrackTitle &&
                   TrackNumber == other.TrackNumber &&
                   Year == other.Year;
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