using System.Collections.Generic;

namespace DeadDog.Audio
{
    public class PlayQueue<T> : IPlayQueue<T>
    {
        private readonly QueueCompare comparer = new QueueCompare();
        private readonly List<QueueEntry<T>> queue;
        private QueueEntry<T> lastDequeued;

        public PlayQueue()
        {
            queue = new List<QueueEntry<T>>();
        }

        public QueueEntry<T> this[int index]
        {
            get { return queue[index]; }
        }

        public virtual void Enqueue(PlaylistEntry<T> entry)
        {
            var e = new QueueEntry<T>(entry);
            int index = queue.BinarySearch(e, comparer);
            queue.Insert(~index, e);
        }

        public virtual PlaylistEntry<T> Dequeue()
        {
            lastDequeued = queue[0];
            queue.RemoveAt(0);
            return lastDequeued.Entry;
        }

        public virtual bool Remove(PlaylistEntry<T> item)
        {
            int c = 0;
            for (int i = 0; i - c < queue.Count; i++)
            {
                if (queue[i].Entry == item)
                {
                    queue.RemoveAt(i - c);
                    c++;
                }
            }
            return c > 0;
        }

        public virtual void Clear()
        {
            queue.Clear();
            lastDequeued = null;
        }

        public int Count
        {
            get { return queue.Count; }
        }

        public PlaylistEntry<T> Peek()
        {
            return queue[0].Entry;
        }

        public virtual void RemoveAt(int index)
        {
            queue.RemoveAt(index);
        }

        public bool Contains(PlaylistEntry<T> item)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Entry == item)
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