namespace DeadDog.Audio
{
    public class QueueEntry<T>
    {
        private static int nextid;
        private readonly PlaylistEntry<T> entry;
        private readonly int id;

        public QueueEntry(PlaylistEntry<T> entry)
        {
            this.entry = entry;
            id = nextid;
            nextid++;
        }

        public PlaylistEntry<T> Entry
        {
            get { return entry; }
        }

        public int CompareTo(QueueEntry<T> x)
        {
            return id.CompareTo(x.id);
        }
    }
}