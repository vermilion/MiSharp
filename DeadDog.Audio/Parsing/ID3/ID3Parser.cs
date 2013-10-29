using System;
using TagLib;

namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Provides a method for reading metadata from files with an ID3 tag.
    /// </summary>
    public class ID3Parser : IDataParser
    {
        /// <summary>
        ///     Reads metadata from files with an ID3 tag.
        /// </summary>
        /// <param name="filepath">The full path of the file from which to read.</param>
        /// <returns>
        ///     A <see cref="RawTrack" /> containing the parsed metadata, if parsing succeeded. If parsing fails an exception is thrown.
        /// </returns>
        public RawTrack ParseTrack(string filepath)
        {
            File tagFile = File.Create(filepath);
            Tag tag = tagFile.GetTag(TagTypes.Id3v2);

            if (tag == null)
                throw new Exception("No tags found.");

            return new Mp3Track(filepath, tag);
        }
    }
}