using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries.Events
{
    /// <summary>
    ///     Represents a method that handles the <see cref="ArtistCollection.ArtistAdded" /> and the
    ///     <see
    ///         cref="ArtistCollection.ArtistRemoved" />
    ///     events.
    /// </summary>
    /// <param name="collection">The artist collection.</param>
    /// <param name="e">
    ///     The <see cref="ArtistEventArgs" /> instance containing the event data.
    /// </param>
    public delegate void ArtistEventHandler(ArtistCollection collection, ArtistEventArgs e);
}