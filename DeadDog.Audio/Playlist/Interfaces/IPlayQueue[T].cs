namespace DeadDog.Audio
{
    public interface IPlayQueue<T>
    {
        int Count { get; }
        void Enqueue(PlaylistEntry<T> entry);
        PlaylistEntry<T> Dequeue();

        bool Remove(PlaylistEntry<T> item);

        void Clear();
    }
}