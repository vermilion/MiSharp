namespace DeadDog.Audio.Playlist
{
    public abstract class QueueFactory<T, Q>
    {
        public abstract QueueEntry<T, Q> Construct(T entry, Q queueinfo);

        public abstract QueueEntry<T, Q> Construct(T entry);
    }
}