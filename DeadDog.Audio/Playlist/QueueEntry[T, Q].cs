namespace DeadDog.Audio
{
    public abstract class QueueEntry<T, Q>
    {
        private static int nextid;
        private readonly PlaylistEntry<T> entry;
        private readonly int id;
        private readonly Q queueinfo;

        public QueueEntry(PlaylistEntry<T> entry, Q queueinfo)
        {
            this.entry = entry;
            this.queueinfo = queueinfo;
            id = nextid;
            nextid++;
        }

        public PlaylistEntry<T> Entry
        {
            get { return entry; }
        }

        public Q QueueInfo
        {
            get { return queueinfo; }
        }

        public abstract int CompareTo(QueueEntry<T, Q> x);

        public int CompareByAddedOrder(QueueEntry<T, Q> x)
        {
            return id.CompareTo(x.id);
        }
    }
}