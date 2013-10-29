namespace DeadDog.Audio.Playlist
{
    public abstract class QueueEntry<T, Q>
    {
        private static int nextid;
        private readonly T _entry;
        private readonly int _id;
        private readonly Q _queueinfo;

        protected QueueEntry(T entry, Q queueinfo)
        {
            _entry = entry;
            _queueinfo = queueinfo;
            _id = nextid;
            nextid++;
        }

        public T Entry
        {
            get { return _entry; }
        }

        public Q QueueInfo
        {
            get { return _queueinfo; }
        }

        public abstract int CompareTo(QueueEntry<T, Q> x);

        public int CompareByAddedOrder(QueueEntry<T, Q> x)
        {
            return _id.CompareTo(x._id);
        }
    }
}