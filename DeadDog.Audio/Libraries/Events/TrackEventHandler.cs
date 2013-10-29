using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries.Events
{
    /// <summary>
    ///     Represents a method that handles the <see cref="TrackCollection.TrackAdded" /> and the
    ///     <see
    ///         cref="TrackCollection.TrackRemoved" />
    ///     events.
    /// </summary>
    /// <param name="collection">The track collection.</param>
    /// <param name="e">
    ///     The <see cref="TrackEventArgs" /> instance containing the event data.
    /// </param>
    public delegate void TrackEventHandler(TrackCollection collection, TrackEventArgs e);
}