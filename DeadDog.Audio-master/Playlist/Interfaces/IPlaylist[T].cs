using System.Collections.Generic;

namespace DeadDog.Audio
{
    /// <summary>
    ///     Represents a collection of objects that can be iterated through a selection of commands.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPlaylist<T> : IEnumerable<PlaylistEntry<T>>
    {
        /// <summary>
        ///     Gets the currently selected <see cref="PlaylistEntry{T}" />.
        /// </summary>
        /// <value>
        ///     The currently selected <see cref="PlaylistEntry{T}" />.
        /// </value>
        PlaylistEntry<T> CurrentEntry { get; }

        /// <summary>
        ///     Moves to the next item in the playlist.
        /// </summary>
        /// <returns>true, if the move was successful; otherwise false.</returns>
        bool MoveNext();

        /// <summary>
        ///     Moves to the previous item in the playlist.
        /// </summary>
        /// <returns>true, if the move was successful; otherwise false.</returns>
        bool MovePrevious();

        /// <summary>
        ///     Moves to a random item in the playlist.
        /// </summary>
        /// <returns>true, if the move was successful; otherwise false.</returns>
        bool MoveRandom();

        bool MoveToFirst();
        bool MoveToLast();

        bool MoveToEntry(PlaylistEntry<T> entry);
        bool Contains(PlaylistEntry<T> entry);

        /// <summary>
        ///     Resets the playlist.
        /// </summary>
        void Reset();
    }
}