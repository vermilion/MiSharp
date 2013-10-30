using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeadDog.Audio.Playlist.Interfaces;
using Rareform.Collections;

namespace DeadDog.Audio.Playlist
{
    public class PlaylistCollection<T> : ObservableList<IPlaylist<T>>, IPlaylist<T>
    {
        private int _index = -1;

        private bool _isSorted;
        private Comparison<IPlaylist<T>> _sortMethod;


        public T CurrentEntry
        {
            get { return _index < 0 ? default(T) : this[_index].CurrentEntry; }
        }

        public bool MoveNext()
        {
            if (_index == -2)
                return false;
            if (Count == 0)
            {
                _index = -2;
                return false;
            }
            if (_index == -1)
            {
                _index = 0;
                this[_index].Reset();
            }

            if (!this[_index].MoveNext())
            {
                _index++;
                if (_index >= Count)
                {
                    _index = -2;
                    return false;
                }
                this[_index].Reset();
                return MoveNext();
            }
            return true;
        }

        public bool MovePrevious()
        {
            if (_index == -2)
                return false;

            if (Count == 0 || _index == -1)
            {
                _index = -2;
                return false;
            }

            if (!this[_index].MovePrevious())
            {
                _index--;
                if (_index < 0)
                {
                    _index = -2;
                    return false;
                }
                this[_index].Reset();
                if (!this[_index].MoveToLast())
                    return MovePrevious();
            }
            return true;
        }

        public bool MoveRandom()
        {
            var rnd = new Random();
            var temp = new List<IPlaylist<T>>(this);

            while (temp.Count > 0)
            {
                int i = rnd.Next(temp.Count);
                if (temp[i].MoveRandom())
                {
                    _index = i;
                    return true;
                }
                temp.RemoveAt(i);
            }
            _index = -2;
            return false;
        }

        public bool MoveToFirst()
        {
            if (Count == 0)
            {
                _index = -2;
                return false;
            }

            _index = 0;
            while (!this[_index].MoveToFirst())
            {
                _index++;
                if (_index >= Count)
                {
                    _index = -2;
                    return false;
                }
            }
            return true;
        }

        public bool MoveToLast()
        {
            if (Count == 0)
            {
                _index = -2;
                return false;
            }

            _index = Count - 1;
            while (!this[_index].MoveToLast())
            {
                _index--;
                if (_index < 0)
                {
                    _index = -2;
                    return false;
                }
            }
            return true;
        }

        public bool MoveToEntry(T entry)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].MoveToEntry(entry))
                {
                    _index = i;
                    return true;
                }

            _index = -2;
            return false;
        }

        public bool Contains(T entry)
        {
            foreach (var playlist in this)
                if (Enumerable.Contains(playlist, entry))
                    return true;
            return false;
        }


        public void Reset()
        {
            _index = -1;
        }

        protected void SetSortMethod(Comparison<IPlaylist<T>> method)
        {
            if (method == null)
                throw new ArgumentNullException("Sort method cannot be null.");
            _isSorted = true;
            _sortMethod = method;

            IPlaylist<T> current = _index >= 0 ? this[_index] : null;
            Sort();
            if (current != null)
                _index = IndexOf(current);
        }

        protected IPlaylist<T> GetPlaylist(int index)
        {
            return this[index];
        }

        protected bool Move(IPlaylist<T> playlist, int index)
        {
            int i = IndexOf(playlist);

            if (i == -1)
                return false;

            IPlaylist<T> selected = index < 0 ? null : this[_index];

            RemoveAt(i);
            Insert(index, playlist);

            if (selected != null)
                _index = IndexOf(selected);
            return true;
        }

        protected void AddPlaylist(IPlaylist<T> playlist)
        {
            if (_isSorted)
            {
                int i = this.BinarySearch(playlist, _sortMethod);
                if (i >= 0 && this[i] == playlist)
                    throw new ArgumentException("A playlist cannot contain the same playlist twice.");
                if (i < 0)
                    i = ~i;

                Insert(i, playlist);
                if (i <= _index)
                    _index++;
            }
            else
                Add(playlist);
        }

        protected bool RemovePlaylist(IPlaylist<T> playlist)
        {
            bool removed = Remove(playlist);
            if (_index >= Count)
                _index = -2;
            else
                this[_index].Reset();
            return removed;
        }

        #region IEnumerable Members

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var playlist in this)
                foreach (T t in playlist)
                    yield return t;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}