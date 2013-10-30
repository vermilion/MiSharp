namespace DeadDog.Audio
{
    /// <summary>
    ///     Represents the method that defines a set of criteria and determines whether the specified object meets those
    ///     criteria.
    /// </summary>
    /// <remarks>
    ///     The criteria are defined as; the object must contain the value specified by the <paramref name="searchstring" />
    ///     parameter.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the object to compare.This type parameter is contravariant.
    ///     That is, you can use either the type you specified or any type that is less derived.
    ///     For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    /// <param name="obj">The object to compare against the criteria defined within the method represented by this delegate.</param>
    /// <param name="searchstring">The string to compare againts the object.</param>
    /// <returns>
    ///     true if <paramref name="obj" /> contains <paramref name="searchstring" />; otherwise, false
    /// </returns>
    public delegate bool PredicateString<in T>(T obj, string searchstring);
}