namespace DeadDog.Audio
{
    public class PlaylistEntry<T>
    {
        private readonly T track;

        public PlaylistEntry(T track)
        {
            this.track = track;
        }

        public T Track
        {
            get { return track; }
        }

        public override string ToString()
        {
            return track.ToString();
        }
    }
}