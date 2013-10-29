using System;
using System.Collections;
using System.Collections.Generic;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Libraries.Collections;
using DeadDog.Audio.Libraries.Events;
using DeadDog.Audio.Playlist.Interfaces;

namespace DeadDog.Audio.Playlist
{
    public class AlbumPlaylist : IPlaylist<Track>
    {
        private readonly Album _album;
        private readonly List<Track> _entries;
        private int _index;
        private Comparison<Track> _sort;

        public AlbumPlaylist(Album album)
        {
            _album = album;
            _entries = new List<Track>();

            foreach (Track track in album.Tracks)
            {
                _entries.Add(track);
            }

            _album.Tracks.TrackAdded += Tracks_TrackAdded;
            _album.Tracks.TrackRemoved += Tracks_TrackRemoved;

            SetSortMethod(DefaultSort);
        }

        public static Comparison<Track> DefaultSort
        {
            get { return Compare; }
        }

        public Track CurrentEntry
        {
            get { return _index < 0 ? null : _entries[_index]; }
        }

        public bool MoveNext()
        {
            if (_index == -2)
                return false;

            _index++;
            if (_index < _entries.Count)
                return true;
            else
            {
                _index = -2;
                return false;
            }
        }

        public bool MovePrevious()
        {
            if (_index == -2)
                return false;

            _index--;
            if (_index < 0)
                return true;
            else
            {
                _index = -2;
                return false;
            }
        }

        public bool MoveRandom()
        {
            if (_entries.Count > 0)
            {
                var r = new Random();
                _index = r.Next(_entries.Count);
                return true;
            }
            else
            {
                _index = -2;
                return false;
            }
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool MoveToFirst()
        {
            if (_entries.Count == 0)
                return false;
            else
            {
                _index = 0;
                return true;
            }
        }

        public bool MoveToLast()
        {
            if (_entries.Count == 0)
            {
                _index = -2;
                return false;
            }
            else
            {
                _index = _entries.Count;
                return true;
            }
        }

        public bool MoveToEntry(Track entry)
        {
            if (_entries.Contains(entry))
            {
                _index = _entries.IndexOf(entry);
                return true;
            }
            else
            {
                _index = -2;
                return false;
            }
        }

        public bool Contains(Track entry)
        {
            if (_entries.Contains(entry))
                return true;
            else return false;
        }

        private void Tracks_TrackAdded(TrackCollection collection, TrackEventArgs e)
        {
            int i = _entries.BinarySearch(e.Track, _sort, x => x);
            if (i >= 0 && _entries[i] == e.Track)
                throw new ArgumentException("A playlist cannot contain the same track twice");
            else if (i < 0)
                i = ~i;

            _entries.Insert(i, e.Track);
            if (i <= _index)
                _index++;
        }

        private void Tracks_TrackRemoved(TrackCollection collection, TrackEventArgs e)
        {
            int i = _entries.BinarySearch(e.Track, _sort, x => x);
            if (i >= 0)
            {
                _entries.RemoveAt(i);
                if (i < _index)
                    _index--;
                else if (_index >= _entries.Count)
                    _index = -2;
            }
            else throw new ArgumentException("Playlist did not contain the track");
        }

        public void SetSortMethod(Comparison<Track> sort)
        {
            if (sort == null)
                throw new ArgumentNullException("Sortmethod cannot be null. Consider setting to the DefaultSort Method");
            else
            {
                _sort = sort;
                Track track = _entries[_index];
                SortEntries();
                _index = _entries.IndexOf(track);
            }
        }

        private void SortEntries()
        {
            _entries.Sort((x, y) => _sort(x, y));
        }

        private static int Compare(Track element1, Track element2)
        {
            int? v1 = element1.Tracknumber, v2 = element2.Tracknumber;
            if (v1.HasValue)
                return v2.HasValue ? v1.Value.CompareTo(v2.Value) : 1;
            else
                return v2.HasValue ? -1 : 0;
        }

        #region IEnumerable<PlaylistEntry<Track>> Members

        IEnumerator<Track> IEnumerable<Track>.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        #endregion
    }
}