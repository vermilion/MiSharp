using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries
{
    /// <summary>
    ///     Represents a method that handles the <see cref="AlbumCollection.AlbumAdded" /> and the
    ///     <see
    ///         cref="AlbumCollection.AlbumRemoved" />
    ///     events.
    /// </summary>
    /// <param name="collection">The album collection.</param>
    /// <param name="e">
    ///     The <see cref="AlbumEventArgs" /> instance containing the event data.
    /// </param>
    public delegate void AlbumEventHandler(AlbumCollection collection, AlbumEventArgs e);
}