namespace DeadDog.Audio.Playlist
{
    public class QueueEntry<T>
    {
        private static int nextid;
        private readonly T _entry;
        private readonly int _id;

        public QueueEntry(T entry)
        {
            _entry = entry;
            _id = nextid;
            nextid++;
        }

        public T Entry
        {
            get { return _entry; }
        }

        public int CompareTo(QueueEntry<T> x)
        {
            return _id.CompareTo(x._id);
        }
    }
}