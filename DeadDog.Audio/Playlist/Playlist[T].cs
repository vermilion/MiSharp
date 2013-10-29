using System;
using System.Collections;
using System.Collections.Generic;
using DeadDog.Audio.Playlist.Interfaces;
using Rareform.Collections;

namespace DeadDog.Audio.Playlist
{
    public class Playlist<T> : ObservableList<T>, IPlaylist<T>, INamedPlaylist
    {
        public Playlist()
        {
            CurrentIndex = -1;
        }

        public int CurrentIndex { get; set; }

        public T CurrentEntry
        {
            get { return CurrentIndex < 0 ? default(T) : this[CurrentIndex]; }
        }

        public bool MoveNext()
        {
            if (CurrentIndex == -2)
                return false;

            CurrentIndex++;
            if (CurrentIndex >= Count)
            {
                CurrentIndex = -2;
                return false;
            }
            else
                return true;
        }

        public bool MovePrevious()
        {
            if (CurrentIndex == -2)
                return false;

            CurrentIndex--;
            if (CurrentIndex == -1)
            {
                CurrentIndex = -2;
                return false;
            }
            else
                return true;
        }

        public bool MoveRandom()
        {
            if (Count == 0)
            {
                CurrentIndex = -2;
                return false;
            }

            var rnd = new Random();
            CurrentIndex = rnd.Next(Count);
            return true;
        }

        public bool MoveToFirst()
        {
            if (this.Count == 0)
            {
                CurrentIndex = -2;
                return false;
            }
            else
            {
                CurrentIndex = 0;
                return true;
            }
        }

        public bool MoveToLast()
        {
            if (Count == 0)
            {
                CurrentIndex = -2;
                return false;
            }
            else
            {
                CurrentIndex = Count - 1;
                return true;
            }
        }

        public bool MoveToEntry(T entry)
        {
            int i = IndexOf(entry);
            if (i == -1)
            {
                CurrentIndex = -2;
                return false;
            }
            else
            {
                CurrentIndex = i;
                return true;
            }
        }

        public void Reset()
        {
            CurrentIndex = -1;
        }

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public string Name { get; set; }
    }
}