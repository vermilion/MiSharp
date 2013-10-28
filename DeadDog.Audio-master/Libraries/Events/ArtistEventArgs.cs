using System;

namespace DeadDog.Audio.Libraries
{
    /// <summary>
    ///     Provides data for the <see cref="DeadDog.Audio.Libraries.Artist.ArtistCollection.ArtistAdded" /> and the
    ///     <see
    ///         cref="DeadDog.Audio.Libraries.Artist.ArtistCollection.ArtistRemoved" />
    ///     events.
    /// </summary>
    public class ArtistEventArgs : EventArgs
    {
        private readonly Artist artist;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArtistEventArgs" /> class.
        /// </summary>
        /// <param name="artist">The artist.</param>
        public ArtistEventArgs(Artist artist)
        {
            this.artist = artist;
        }

        /// <summary>
        ///     Gets the artist associated with the event.
        /// </summary>
        /// <value>
        ///     The artist.
        /// </value>
        public Artist Artist
        {
            get { return artist; }
        }
    }
}