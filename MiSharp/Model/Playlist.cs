using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Rareform.Validation;

namespace MiSharp
{
    public class Playlist : IList<PlaylistEntry>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<PlaylistEntry> _playlist;

        public Playlist()
        {
            _playlist = new ObservableCollection<PlaylistEntry>();
            Name = "Default";
        }

        public int CurrentSongIndex { get; set; }

        public string Name { get; set; }

        public IEnumerator<PlaylistEntry> GetEnumerator()
        {
            return _playlist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PlaylistEntry item)
        {
            _playlist.Add(item);
        }

        public void Clear()
        {
            _playlist.Clear();
        }

        public bool Contains(PlaylistEntry item)
        {
            return _playlist.Any(entry => entry == item);
        }

        public void CopyTo(PlaylistEntry[] array, int arrayIndex)
        {
            _playlist.CopyTo(array, arrayIndex);
        }

        public bool Remove(PlaylistEntry item)
        {
            return _playlist.Remove(item);
        }

        public int Count
        {
            get { return _playlist.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(PlaylistEntry item)
        {
            return _playlist.IndexOf(item);
        }

        public void Insert(int index, PlaylistEntry item)
        {
            _playlist.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _playlist.RemoveAt(index);
        }

        public PlaylistEntry this[int index]
        {
            get
            {
                if (index < 0)
                    Throw.ArgumentOutOfRangeException(() => index, 0);

                int maxIndex = _playlist.Count - 1;

                if (index > maxIndex)
                    Throw.ArgumentOutOfRangeException(() => index, maxIndex);

                return _playlist[index];
            }
            set { _playlist[index] = value; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _playlist.CollectionChanged += value; }
            remove { _playlist.CollectionChanged -= value; }
        }
    }
}