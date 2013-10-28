using System.IO;
using System.Text;

namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Provides methods for parsing ID3v1 metadata from audio files.
    /// </summary>
    public class ID3v1
    {
        private static readonly Encoding iso = Encoding.GetEncoding("ISO-8859-1");

        private static readonly ID3v1 empty;

        private string album;
        private string artist;
        private string comment;
        private bool tagfound;
        private string title;
        private int tracknumber = -1;
        private string year;

        static ID3v1()
        {
            empty = new ID3v1();
            empty.tagfound = false;
            empty.title = null;
            empty.artist = null;
            empty.album = null;
            empty.year = null;
            empty.comment = null;
            empty.tracknumber = -1;
        }

        private ID3v1()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ID3v1" /> class from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">
        ///     A <see cref="Stream" /> from which the metadata is read. The position within the stream is reset after reading.
        /// </param>
        public ID3v1(Stream stream)
            : this(read128(stream))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ID3v1" /> class.
        /// </summary>
        /// <param name="filename">The path of the file to read metadata from.</param>
        public ID3v1(string filename)
            : this(read128(filename))
        {
        }

        private ID3v1(byte[] buffer)
        {
            if (buffer.Length < 128)
            {
                tagfound = false;
                return;
            }
            else if (iso.GetString(buffer, 0, 3) != "TAG")
            {
                tagfound = false;
                return;
            }
            else
                tagfound = true;

            title = iso.GetString(buffer, 3, 30).Trim('\0', ' ');
            artist = iso.GetString(buffer, 33, 30).Trim('\0', ' ');
            album = iso.GetString(buffer, 63, 30).Trim('\0', ' ');
            year = iso.GetString(buffer, 93, 4).Trim('\0', ' ');

            bool hasTrack = (buffer[125] == 0);
            if (hasTrack)
            {
                comment = iso.GetString(buffer, 97, 28).Trim('\0', ' ');
                tracknumber = buffer[126];
            }
            else
            {
                comment = iso.GetString(buffer, 97, 30).Trim('\0', ' ');
            }
        }

        /// <summary>
        ///     Gets a <see cref="ID3v1" /> where all values have been set do their default.
        /// </summary>
        public static ID3v1 Empty
        {
            get { return empty; }
        }

        /// <summary>
        ///     Gets a value indicating whether an ID3v1 tag could be found.
        /// </summary>
        public bool TagFound
        {
            get { return tagfound; }
        }

        /// <summary>
        ///     Gets the name of the artist, if it could be loaded, null if not.
        /// </summary>
        public string Artist
        {
            get { return artist; }
        }

        /// <summary>
        ///     Gets the title of the album, if it could be loaded, null if not.
        /// </summary>
        public string Album
        {
            get { return album; }
        }

        /// <summary>
        ///     Gets the title of the track, if it could be loaded, null if not.
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        ///     Gets the tracknumber (on album) for this file, if it could be loaded, -1 if not.
        /// </summary>
        public int TrackNumber
        {
            get { return tracknumber; }
        }

        /// <summary>
        ///     Gets the release-year of the track, if it could be loaded, null if not.
        /// </summary>
        public string Year
        {
            get { return year; }
        }

        private static byte[] read128(Stream stream)
        {
            if (stream.Length < 128)
                return new byte[0];

            long position = stream.Position;

            var buffer = new byte[128];
            stream.Seek(-128, SeekOrigin.End);
            stream.Read(buffer, 0, 128);

            stream.Seek(position, SeekOrigin.Begin);

            return buffer;
        }

        private static byte[] read128(string filename)
        {
            byte[] buffer;
            using (Stream stream = File.Open(filename, FileMode.Open))
                buffer = read128(stream);

            return buffer;
        }
    }
}