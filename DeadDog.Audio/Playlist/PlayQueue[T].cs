using System.Collections.Generic;
using DeadDog.Audio.Playlist.Interfaces;

namespace DeadDog.Audio.Playlist
{
    public class PlayQueue<T> : IPlayQueue<T>
    {
        private readonly QueueCompare _comparer = new QueueCompare();
        private readonly List<QueueEntry<T>> _queue;
        private QueueEntry<T> _lastDequeued;

        public PlayQueue()
        {
            _queue = new List<QueueEntry<T>>();
        }

        public QueueEntry<T> this[int index]
        {
            get { return _queue[index]; }
        }

        public virtual void Enqueue(T entry)
        {
            var e = new QueueEntry<T>(entry);
            int index = _queue.BinarySearch(e, _comparer);
            _queue.Insert(~index, e);
        }

        public virtual T Dequeue()
        {
            _lastDequeued = _queue[0];
            _queue.RemoveAt(0);
            return _lastDequeued.Entry;
        }

        public virtual bool Remove(T item)
        {
            int c = 0;
            for (int i = 0; i - c < _queue.Count; i++)
            {
                if (_queue[i].Entry.Equals(item))
                {
                    _queue.RemoveAt(i - c);
                    c++;
                }
            }
            return c > 0;
        }

        public virtual void Clear()
        {
            _queue.Clear();
            _lastDequeued = null;
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public T Peek()
        {
            return _queue[0].Entry;
        }

        public virtual void RemoveAt(int index)
        {
            _queue.RemoveAt(index);
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Entry.Equals(item))
                    return true;
            }
            return false;
        }

        private class QueueCompare : IComparer<QueueEntry<T>>
        {
            public int Compare(QueueEntry<T> x, QueueEntry<T> y)
            {
                return x.CompareTo(y);
            }
        }
    }
}