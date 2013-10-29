using System;
using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries.Events
{
    /// <summary>
    ///     Provides data for the <see cref="ArtistCollection.ArtistAdded" /> and the
    ///     <see
    ///         cref="ArtistCollection.ArtistRemoved" />
    ///     events.
    /// </summary>
    public class ArtistEventArgs : EventArgs
    {
        private readonly Artist _artist;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArtistEventArgs" /> class.
        /// </summary>
        /// <param name="artist">The artist.</param>
        public ArtistEventArgs(Artist artist)
        {
            this._artist = artist;
        }

        /// <summary>
        ///     Gets the artist associated with the event.
        /// </summary>
        /// <value>
        ///     The artist.
        /// </value>
        public Artist Artist
        {
            get { return _artist; }
        }
    }
}