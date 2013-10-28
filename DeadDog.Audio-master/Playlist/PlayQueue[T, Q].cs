using System.Collections.Generic;

namespace DeadDog.Audio
{
    public class PlayQueue<T, Q> : IPlayQueue<T>
    {
        private readonly QueueCompare comparer = new QueueCompare();
        private readonly QueueFactory<T, Q> factory;
        private readonly List<QueueEntry<T, Q>> queue;
        private QueueEntry<T, Q> lastDequeued;

        public PlayQueue(QueueFactory<T, Q> factory)
        {
            this.factory = factory;
            queue = new List<QueueEntry<T, Q>>();
        }

        public QueueEntry<T, Q> this[int index]
        {
            get { return queue[index]; }
        }

        public virtual void Enqueue(PlaylistEntry<T> entry)
        {
            QueueEntry<T, Q> e = factory.Construct(entry);
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

        public virtual void Enqueue(PlaylistEntry<T> entry, Q queueInfo)
        {
            QueueEntry<T, Q> e = factory.Construct(entry, queueInfo);
            int index = queue.BinarySearch(e, comparer);
            queue.Insert(~index, e);
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

        private class QueueCompare : IComparer<QueueEntry<T, Q>>
        {
            public int Compare(QueueEntry<T, Q> x, QueueEntry<T, Q> y)
            {
                return x.CompareTo(y);
            }
        }
    }
}