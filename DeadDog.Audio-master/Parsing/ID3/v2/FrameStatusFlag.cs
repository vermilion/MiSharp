namespace DeadDog.Audio.Parsing.ID3
{
    /// <summary>
    ///     Defines constants for ID3v2 frame status flagbyte.
    /// </summary>
    public enum FrameStatusFlag
    {
        /// <summary>
        ///     This flag tells the tag parser what to do with this frame if it is
        ///     unknown and the tag is altered in any way. This applies to all
        ///     kinds of alterations, including adding more padding and reordering
        ///     the frames.
        /// </summary>
        TagAlterPreservation = 64,

        /// <summary>
        ///     This flag tells the tag parser what to do with this frame if it is
        ///     unknown and the file, excluding the tag, is altered. This does not
        ///     apply when the audio is completely replaced with other audio data.
        /// </summary>
        FileAlterPreservation = 32,

        /// <summary>
        ///     This flag, if set, tells the software that the contents of this
        ///     frame are intended to be read only. Changing the contents might
        ///     break something, e.g. a signature. If the contents are changed,
        ///     without knowledge of why the frame was flagged read only and
        ///     without taking the proper means to compensate, e.g. recalculating
        ///     the signature, the bit MUST be cleared.
        /// </summary>
        ReadOnly = 16
    }
}