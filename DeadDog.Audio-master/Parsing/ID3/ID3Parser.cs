using System;

namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Provides a method for reading metadata from files with an ID3 tag, using an <see cref="ID3info" /> object.
    /// </summary>
    public class ID3Parser : IDataParser
    {
        /// <summary>
        ///     Reads metadata from files with an ID3 tag, using an <see cref="ID3info" /> object.
        /// </summary>
        /// <param name="filepath">The full path of the file from which to read.</param>
        /// <returns>
        ///     A <see cref="RawTrack" /> containing the parsed metadata, if parsing succeeded. If parsing fails an exception is thrown.
        /// </returns>
        public RawTrack ParseTrack(string filepath)
        {
            var id3 = new ID3info(filepath);
            if (!id3.ID3v1.TagFound && !id3.ID3v2.TagFound)
                throw new Exception("No tags found.");

            return new RawTrack(filepath, id3.Title, id3.Album, id3.TrackNumber, id3.Artist, id3.Year);
        }
    }
}