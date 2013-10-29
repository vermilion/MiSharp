namespace DeadDog.Audio.Playlist.Interfaces
{
    public interface IQueuePeekable<T> : IPlayQueue<T>
    {
        T Peek();
    }
}