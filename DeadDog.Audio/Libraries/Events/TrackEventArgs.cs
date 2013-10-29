using System;
using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries.Events
{
    /// <summary>
    ///     Provides data for the <see cref="TrackCollection.TrackAdded" /> and the
    ///     <see
    ///         cref="TrackCollection.TrackRemoved" />
    ///     events.
    /// </summary>
    public class TrackEventArgs : EventArgs
    {
        private readonly Track _track;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackEventArgs" /> class.
        /// </summary>
        /// <param name="track">The track.</param>
        public TrackEventArgs(Track track)
        {
            this._track = track;
        }

        /// <summary>
        ///     Gets the track associated with the event.
        /// </summary>
        /// <value>
        ///     The track.
        /// </value>
        public Track Track
        {
            get { return _track; }
        }
    }
}