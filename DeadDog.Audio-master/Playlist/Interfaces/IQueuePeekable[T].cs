namespace DeadDog.Audio
{
    public interface IQueuePeekable<T> : IPlayQueue<T>
    {
        PlaylistEntry<T> Peek();
    }
}