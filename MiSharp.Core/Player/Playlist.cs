using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiSharp.Model;
using Rareform.Reflection;
using Rareform.Validation;

namespace MiSharp
{
    /// <summary>
    ///     Represents a playlist where songs are stored with an associated index.
    /// </summary>
    public sealed class Playlist : IEnumerable<PlaylistEntry>
    {
        private readonly List<PlaylistEntry> _playlist;
        private int? _currentSongIndex;

        internal Playlist(string name)
        {
            Name = name;
            _playlist = new List<PlaylistEntry>();
        }

        /// <summary>
        ///     Gets a value indicating whether the next song in the playlist can be played.
        /// </summary>
        /// <value>
        ///     true if the next song in the playlist can be played; otherwise, false.
        /// </value>
        public bool CanPlayNextSong
        {
            get { return CurrentSongIndex.HasValue && ContainsIndex(CurrentSongIndex.Value + 1); }
        }

        /// <summary>
        ///     Gets a value indicating whether the previous song in the playlist can be played.
        /// </summary>
        /// <value>
        ///     true if the previous song in the playlist can be played; otherwise, false.
        /// </value>
        public bool CanPlayPreviousSong
        {
            get { return CurrentSongIndex.HasValue && ContainsIndex(CurrentSongIndex.Value - 1); }
        }

        /// <summary>
        ///     Gets the index of the currently played song in the playlist.
        /// </summary>
        /// <value>
        ///     The index of the currently played song in the playlist. <c>null</c>, if no song is currently played.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">The value is not in the range of the playlist's indexes.</exception>
        public int? CurrentSongIndex
        {
            get { return _currentSongIndex; }
            internal set
            {
                if (value != null && !ContainsIndex(value.Value))
                    Throw.ArgumentOutOfRangeException(() => value);

                _currentSongIndex = value;
            }
        }

        public string Name { get; set; }

        public PlaylistEntry this[int index]
        {
            get
            {
                if (index < 0)
                    Throw.ArgumentOutOfRangeException(() => index, 0);

                int maxIndex = _playlist.Count;

                if (index > maxIndex)
                    Throw.ArgumentOutOfRangeException(() => index, maxIndex);

                return _playlist[index];
            }
        }

        public IEnumerator<PlaylistEntry> GetEnumerator()
        {
            return _playlist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Gets a value indicating whether there exists a song at the specified index.
        /// </summary>
        /// <param name="songIndex">The index to look for.</param>
        /// <returns>True, if there exists a song at the specified index; otherwise, false.</returns>
        public bool ContainsIndex(int songIndex)
        {
            return _playlist.Any(entry => entry.Index == songIndex);
        }

        /// <summary>
        ///     Gets all indexes of the specified songs.
        /// </summary>
        public IEnumerable<int> GetIndexes(IEnumerable<Song> songs)
        {
            return _playlist
                .Where(entry => songs.Contains(entry.Song))
                .Select(entry => entry.Index)
                .ToList();
        }

        /// <summary>
        ///     Adds the specified songs to end of the playlist.
        /// </summary>
        /// <param name="songList">The songs to add to the end of the playlist.</param>
        internal void AddSongs(IEnumerable<Song> songList)
        {
            if (songList == null)
                Throw.ArgumentNullException(() => songList);

            foreach (Song song in songList)
            {
                _playlist.Add(new PlaylistEntry(_playlist.Count, song));
            }
        }

        /// <summary>
        ///     Inserts a song from a specified index to a other index in the playlist and moves all songs in between these indexes
        ///     one index back.
        /// </summary>
        /// <param name="fromIndex">The index of the song to move.</param>
        /// <param name="toIndex">To index to insert the song.</param>
        internal void InsertMove(int fromIndex, int toIndex)
        {
            if (fromIndex < 0)
                Throw.ArgumentOutOfRangeException(() => fromIndex, 0);

            if (toIndex < 0)
                Throw.ArgumentOutOfRangeException(() => toIndex, 0);

            if (toIndex >= fromIndex)
                Throw.ArgumentException(
                    String.Format("{0} has to be smaller than {1}",
                        Reflector.GetMemberName(() => toIndex), Reflector.GetMemberName(() => fromIndex)),
                    () => toIndex);

            PlaylistEntry from = this[fromIndex];

            for (int i = fromIndex; i > toIndex; i--)
            {
                _playlist[i].Index = i - 1;
                _playlist[i] = this[i - 1];
            }

            from.Index = toIndex;
            _playlist[toIndex] = from;
        }

        /// <summary>
        ///     Removes the songs with the specified indexes from the <see cref="Playlist" />.
        /// </summary>
        /// <param name="indexes">The indexes of the songs to remove.</param>
        internal void RemoveSongs(IEnumerable<int> indexes)
        {
            if (indexes == null)
                Throw.ArgumentNullException(() => indexes);

            // Use a hashset for better lookup performance
            var indexList = new HashSet<int>(indexes);

            if (CurrentSongIndex.HasValue && indexList.Contains(CurrentSongIndex.Value))
            {
                CurrentSongIndex = null;
            }

            _playlist.RemoveAll(entry => indexList.Contains(entry.Index));

            RebuildIndexes();
        }

        internal void Shuffle()
        {
            int count = _playlist.Count;

            var random = new Random();

            for (int index = 0; index < count; index++)
            {
                int newIndex = random.Next(count);

                // Migrate the CurrentSongIndex to the new position
                if (index == CurrentSongIndex)
                {
                    CurrentSongIndex = newIndex;
                }

                else if (newIndex == CurrentSongIndex)
                {
                    CurrentSongIndex = index;
                }

                PlaylistEntry temp = _playlist[index];

                _playlist[newIndex].Index = index;
                _playlist[index] = _playlist[newIndex];

                temp.Index = newIndex;
                _playlist[newIndex] = temp;
            }
        }

        private void RebuildIndexes()
        {
            int index = 0;
            int? migrateIndex = null;
            List<PlaylistEntry> current = _playlist.ToList();

            _playlist.Clear();

            foreach (PlaylistEntry entry in current)
            {
                if (CurrentSongIndex == entry.Index)
                {
                    migrateIndex = index;
                }

                _playlist.Add(entry);
                entry.Index = index;

                index++;
            }

            if (migrateIndex.HasValue)
            {
                CurrentSongIndex = migrateIndex;
            }
        }
    }
}