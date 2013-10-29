namespace DeadDog.Audio
{
    /// <summary>
    ///     Defines constants for playlist file-restriction.
    /// </summary>
    public enum RestrictionTypes
    {
        /// <summary>
        ///     No restriction. All files are played.
        /// </summary>
        None,

        /// <summary>
        ///     Restricted to one file. Only the current file is played.
        /// </summary>
        One,

        /// <summary>
        ///     Restricted to album name. Only files with equal album name are played.
        /// </summary>
        AlbumOnly,

        /// <summary>
        ///     Restricted to artist name. Only files with equal artist name are played.
        /// </summary>
        ArtistOnly
    }
}