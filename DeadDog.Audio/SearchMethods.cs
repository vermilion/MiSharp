namespace DeadDog.Audio
{
    /// <summary>
    ///     Determines the type of search to perform when searching for audio using strings as input.
    /// </summary>
    public enum SearchMethods
    {
        /// <summary>
        ///     Specifies that at least one of the strings searched for must be found in the result.
        ///     Equivalent to <see cref="ContainsAll" /> for zero or one string.
        /// </summary>
        ContainsAny,

        /// <summary>
        ///     Specifies that all of the strings searched for must be found in the result.
        ///     Equivalent to <see cref="ContainsAny" /> for zero or one string.
        /// </summary>
        ContainsAll
    }
}