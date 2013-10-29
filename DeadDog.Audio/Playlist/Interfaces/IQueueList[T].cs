namespace DeadDog.Audio.Playlist.Interfaces
{
    public interface IQueueList<T> : IPlayQueue<T>
    {
        T this[int index] { get; }

        int IndexOf(T item);
        void RemoveAt(int index);

        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
    }
}