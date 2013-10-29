namespace DeadDog.Audio
{
    public interface IQueueList<T> : IPlayQueue<T>
    {
        PlaylistEntry<T> this[int index] { get; }

        int IndexOf(PlaylistEntry<T> item);
        void RemoveAt(int index);

        bool Contains(PlaylistEntry<T> item);
        void CopyTo(PlaylistEntry<T>[] array, int arrayIndex);
    }
}