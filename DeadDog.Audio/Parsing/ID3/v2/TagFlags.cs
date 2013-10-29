namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Defines constants for ID3v2 tag flagbyte.
    /// </summary>
    public enum TagFlags
    {
        /// <summary>
        ///     Indicates whether or not unsynchronisation is applied on all frames.
        /// </summary>
        Unsynchronisation = 128,

        /// <summary>
        ///     Indicates whether or not the header is followed by an extended header.
        /// </summary>
        ExtendedHeader = 64,

        /// <summary>
        ///     Indicates that the tag is at an experimental stage.
        /// </summary>
        ExperimentalIndicator = 32,

        /// <summary>
        ///     Indicates that a footer is present at the very end of the tag.
        /// </summary>
        FooterPresent = 16
    }
}