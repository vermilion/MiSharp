namespace DeadDog.Audio.Parsing
{
    /// <summary>
    ///     Provides generic methods for parsing metadata from files to <see cref="RawTrack" /> items.
    /// </summary>
    public interface IDataParser
    {
        /// <summary>
        ///     When overridden in a derived class, reads metadata from an audio-file.
        /// </summary>
        /// <param name="filepath">The full path of the file from which to read.</param>
        /// <returns>
        ///     When overridden in a derived class, returns a <see cref="RawTrack" /> containing the parsed metadata, if parsing
        ///     succeeded, or null if parsing failed..
        /// </returns>
        RawTrack ParseTrack(string filepath);
    }

    public static class DataParserExtension
    {
        /// <summary>
        ///     Reads metadata from a file to a <see cref="RawTrack" /> item. A return value indicates whether parsing succeeded.
        ///     The actual parsing is done using
        ///     <see
        ///         cref="ParseTrack(string)" />
        ///     .
        /// </summary>
        /// <param name="parser">
        ///     The <see cref="IDataParser" /> used for parsing metadata.
        /// </param>
        /// <param name="filepath">The full path of the file from which to read.</param>
        /// <param name="item">
        ///     When the method returns, contains the read metadata, if parsing succeeded, or null if parsing failed. Parsing fails
        ///     if any exception is thrown from the
        ///     <see
        ///         cref="ParseTrack(string)" />
        ///     method. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the file was parsed successfully; otherwise, false.</returns>
        public static bool TryParseTrack(this IDataParser parser, string filepath, out RawTrack item)
        {
            try
            {
                item = parser.ParseTrack(filepath);
                return item != null;
            }
            catch
            {
                item = null;
                return false;
            }
        }
    }
}