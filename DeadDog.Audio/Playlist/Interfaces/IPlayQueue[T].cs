namespace DeadDog.Audio.Playlist.Interfaces
{
    public interface IPlayQueue<T>
    {
        int Count { get; }
        void Enqueue(T entry);
        T Dequeue();

        bool Remove(T item);

        void Clear();
    }
}