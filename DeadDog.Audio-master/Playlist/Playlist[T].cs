using System;
using System.Collections;
using System.Collections.Generic;

namespace DeadDog.Audio
{
    public class Playlist<T> : IPlaylist<T>, IList<PlaylistEntry<T>>
    {
        private readonly List<PlaylistEntry<T>> entries;
        private int index = -1;

        public Playlist()
        {
            entries = new List<PlaylistEntry<T>>();
        }

        public int CurrentIndex
        {
            get { return index; }
        }

        public PlaylistEntry<T> CurrentEntry
        {
            get { return index < 0 ? null : entries[index]; }
        }

        public bool MoveNext()
        {
            if (index == -2)
                return false;

            index++;
            if (index >= entries.Count)
            {
                index = -2;
                return false;
            }
            else
                return true;
        }

        public bool MovePrevious()
        {
            if (index == -2)
                return false;

            index--;
            if (index == -1)
            {
                index = -2;
                return false;
            }
            else
                return true;
        }

        public bool MoveRandom()
        {
            if (entries.Count == 0)
            {
                index = -2;
                return false;
            }

            var rnd = new Random();
            index = rnd.Next(entries.Count);
            return true;
        }

        public bool MoveToFirst()
        {
            if (entries.Count == 0)
            {
                index = -2;
                return false;
            }
            else
            {
                index = 0;
                return true;
            }
        }

        public bool MoveToLast()
        {
            if (entries.Count == 0)
            {
                index = -2;
                return false;
            }
            else
            {
                index = entries.Count - 1;
                return true;
            }
        }

        public bool MoveToEntry(PlaylistEntry<T> entry)
        {
            int i = entries.IndexOf(entry);
            if (i == -1)
            {
                index = -2;
                return false;
            }
            else
            {
                index = i;
                return true;
            }
        }

        public void Reset()
        {
            index = -1;
        }

        #region IList<PlaylistEntry<T>> Members

        public PlaylistEntry<T> this[int index]
        {
            get { return entries[index]; }
        }

        public int IndexOf(PlaylistEntry<T> item)
        {
            return entries.IndexOf(item);
        }

        public void Insert(int index, PlaylistEntry<T> item)
        {
            entries.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            PlaylistEntry<T> entry = entries[index];
            if (index == this.index && index >= entries.Count - 1)
                this.index = -2;
            else if (index > this.index)
                this.index--;
            entries.Remove(entry);
        }

        PlaylistEntry<T> IList<PlaylistEntry<T>>.this[int index]
        {
            get { return this[index]; }
            set { throw new InvalidOperationException("Property cannot be set."); }
        }

        public int IndexOf(T item)
        {
            return entries.FindIndex(x => x.Track.Equals(item));
        }

        #endregion

        #region ICollection<PlaylistEntry<T>> Members

        public void Add(PlaylistEntry<T> item)
        {
            entries.Add(item);
        }

        public void Clear()
        {
            entries.Clear();
            index = -2;
        }

        void ICollection<PlaylistEntry<T>>.CopyTo(PlaylistEntry<T>[] array, int arrayIndex)
        {
            entries.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return entries.Count; }
        }

        bool ICollection<PlaylistEntry<T>>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(PlaylistEntry<T> item)
        {
            int index = entries.IndexOf(item);
            if (index == -1)
                return false;
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        public bool Contains(PlaylistEntry<T> item)
        {
            return entries.Contains(item);
        }

        #endregion

        #region IEnumerable<PlaylistEntry<T>> Members

        IEnumerator<PlaylistEntry<T>> IEnumerable<PlaylistEntry<T>>.GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        #endregion
    }
}