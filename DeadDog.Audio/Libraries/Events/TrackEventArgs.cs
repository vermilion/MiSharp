using System;

namespace DeadDog.Audio.Libraries
{
    /// <summary>
    ///     Provides data for the <see cref="DeadDog.Audio.Libraries.Track.TrackCollection.TrackAdded" /> and the
    ///     <see
    ///         cref="DeadDog.Audio.Libraries.Track.TrackCollection.TrackRemoved" />
    ///     events.
    /// </summary>
    public class TrackEventArgs : EventArgs
    {
        private readonly Track track;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackEventArgs" /> class.
        /// </summary>
        /// <param name="track">The track.</param>
        public TrackEventArgs(Track track)
        {
            this.track = track;
        }

        /// <summary>
        ///     Gets the track associated with the event.
        /// </summary>
        /// <value>
        ///     The track.
        /// </value>
        public Track Track
        {
            get { return track; }
        }
    }
}