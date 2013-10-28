namespace DeadDog.Audio
{
    public abstract class QueueFactory<T, Q>
    {
        public abstract QueueEntry<T, Q> Construct(PlaylistEntry<T> entry, Q queueinfo);

        public abstract QueueEntry<T, Q> Construct(PlaylistEntry<T> entry);
    }
}