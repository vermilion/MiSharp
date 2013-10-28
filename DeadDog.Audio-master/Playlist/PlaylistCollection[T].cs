using System;
using System.Collections;
using System.Collections.Generic;

namespace DeadDog.Audio
{
    public abstract class PlaylistCollection<T> : IPlaylist<T>
    {
        private readonly List<IPlaylist<T>> playlists;
        private int index = -1;

        private bool isSorted;
        private Comparison<IPlaylist<T>> sortMethod;

        public PlaylistCollection()
        {
            playlists = new List<IPlaylist<T>>();
        }

        protected int count
        {
            get { return playlists.Count; }
        }

        public PlaylistEntry<T> CurrentEntry
        {
            get { return index < 0 ? null : playlists[index].CurrentEntry; }
        }

        public bool MoveNext()
        {
            if (index == -2)
                return false;
            else if (playlists.Count == 0)
            {
                index = -2;
                return false;
            }
            else if (index == -1)
            {
                index = 0;
                playlists[index].Reset();
            }

            if (!playlists[index].MoveNext())
            {
                index++;
                if (index >= playlists.Count)
                {
                    index = -2;
                    return false;
                }
                playlists[index].Reset();
                return MoveNext();
            }
            return true;
        }

        public bool MovePrevious()
        {
            if (index == -2)
                return false;

            if (playlists.Count == 0 || index == -1)
            {
                index = -2;
                return false;
            }

            if (!playlists[index].MovePrevious())
            {
                index--;
                if (index < 0)
                {
                    index = -2;
                    return false;
                }
                playlists[index].Reset();
                if (!playlists[index].MoveToLast())
                    return MovePrevious();
            }
            return true;
        }

        public bool MoveRandom()
        {
            var rnd = new Random();
            var temp = new List<IPlaylist<T>>(playlists);

            while (temp.Count > 0)
            {
                int i = rnd.Next(temp.Count);
                if (temp[i].MoveRandom())
                {
                    index = i;
                    return true;
                }
                else
                    temp.RemoveAt(i);
            }
            index = -2;
            return false;
        }

        public bool MoveToFirst()
        {
            if (playlists.Count == 0)
            {
                index = -2;
                return false;
            }

            index = 0;
            while (!playlists[index].MoveToFirst())
            {
                index++;
                if (index >= playlists.Count)
                {
                    index = -2;
                    return false;
                }
            }
            return true;
        }

        public bool MoveToLast()
        {
            if (playlists.Count == 0)
            {
                index = -2;
                return false;
            }

            index = playlists.Count - 1;
            while (!playlists[index].MoveToLast())
            {
                index--;
                if (index < 0)
                {
                    index = -2;
                    return false;
                }
            }
            return true;
        }

        public bool MoveToEntry(PlaylistEntry<T> entry)
        {
            for (int i = 0; i < playlists.Count; i++)
                if (playlists[i].MoveToEntry(entry))
                {
                    index = i;
                    return true;
                }

            index = -2;
            return false;
        }

        public bool Contains(PlaylistEntry<T> entry)
        {
            for (int i = 0; i < playlists.Count; i++)
                if (playlists[i].Contains(entry))
                    return true;
            return false;
        }

        public void Reset()
        {
            index = -1;
        }

        protected void setSortMethod(Comparison<IPlaylist<T>> method)
        {
            if (method == null)
                throw new ArgumentNullException("Sort method cannot be null.");
            else
            {
                isSorted = true;
                sortMethod = method;

                IPlaylist<T> current = index >= 0 ? playlists[index] : null;
                playlists.Sort(sortMethod);
                if (current != null)
                    index = playlists.IndexOf(current);
            }
        }

        protected IPlaylist<T> getPlaylist(int index)
        {
            return playlists[index];
        }

        protected bool contains(IPlaylist<T> playlist)
        {
            return playlists.Contains(playlist);
        }

        protected bool move(IPlaylist<T> playlist, int index)
        {
            int i = playlists.IndexOf(playlist);

            if (i == -1)
                return false;

            IPlaylist<T> selected = index < 0 ? null : playlists[this.index];

            playlists.RemoveAt(i);
            playlists.Insert(index, playlist);

            if (selected != null)
                this.index = playlists.IndexOf(selected);
            return true;
        }

        protected void addPlaylist(IPlaylist<T> playlist)
        {
            if (isSorted)
            {
                int i = playlists.BinarySearch(playlist, sortMethod);
                if (i >= 0 && playlists[i] == playlist)
                    throw new ArgumentException("A playlist cannot contain the same playlist twice.");
                else if (i < 0)
                    i = ~i;

                playlists.Insert(i, playlist);
                if (i <= index)
                    index++;
            }
            else
                playlists.Add(playlist);
        }

        protected bool removePlaylist(IPlaylist<T> playlist)
        {
            bool removed = playlists.Remove(playlist);
            if (index >= playlists.Count)
                index = -2;
            else
                playlists[index].Reset();
            return removed;
        }

        #region IEnumerable<T> Members

        IEnumerator<PlaylistEntry<T>> IEnumerable<PlaylistEntry<T>>.GetEnumerator()
        {
            foreach (var playlist in playlists)
                foreach (var t in playlist)
                    yield return t;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var playlist in playlists)
                foreach (var t in playlist)
                    yield return t;
        }

        #endregion
    }
}