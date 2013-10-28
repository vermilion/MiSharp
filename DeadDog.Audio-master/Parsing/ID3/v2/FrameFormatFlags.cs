namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Defines constants ID3v2 frame format flagbyte.
    /// </summary>
    public enum FrameFormatFlags
    {
        /// <summary>
        ///     This flag indicates whether or not this frame belongs in a group
        ///     with other frames. If set, a group identifier byte is added to the
        ///     frame. Every frame with the same group identifier belongs to the
        ///     same group.
        /// </summary>
        GroupingIdentity = 64,

        /// <summary>
        ///     This flag indicates whether or not the frame is compressed.
        ///     A 'Data Length Indicator' byte MUST be included in the frame.
        /// </summary>
        Compression = 8,

        /// <summary>
        ///     This flag indicates whether or not the frame is encrypted. If set,
        ///     one byte indicating with which method it was encrypted will be
        ///     added to the frame. See description of the ENCR frame for more
        ///     information about encryption method registration. Encryption
        ///     should be done after compression. Whether or not setting this flag
        ///     requires the presence of a 'Data Length Indicator' depends on the
        ///     specific algorithm used.
        /// </summary>
        Encryption = 4,

        /// <summary>
        ///     This flag indicates whether or not unsynchronisation was applied
        ///     to this frame. See section 6 for details on unsynchronisation.
        ///     If this flag is set all data from the end of this header to the
        ///     end of this frame has been unsynchronised. Although desirable, the
        ///     presence of a 'Data Length Indicator' is not made mandatory by
        ///     unsynchronisation.
        /// </summary>
        Unsynchronisation = 2,

        /// <summary>
        ///     This flag indicates that a data length indicator has been added to
        ///     the frame. The data length indicator is the value one would write
        ///     as the 'Frame length' if all of the frame format flags were
        ///     zeroed, represented as a 32 bit synchsafe integer.
        /// </summary>
        DataLengthIndicator = 1
    }
}