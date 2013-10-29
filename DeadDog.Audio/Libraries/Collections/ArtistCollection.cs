using System;

namespace DeadDog.Audio.Libraries.Collections
{
    public class ArtistCollection : LibraryCollectionBase<Artist>
    {
        internal ArtistCollection()
        {
        }

        public event ArtistEventHandler ArtistAdded , ArtistRemoved;

        protected override void OnAdded(Artist element)
        {
            if (ArtistAdded != null)
                ArtistAdded(this, new ArtistEventArgs(element));
        }

        protected override void OnRemoved(Artist element)
        {
            if (ArtistRemoved != null)
                ArtistRemoved(this, new ArtistEventArgs(element));
        }

        protected override string GetName(Artist element)
        {
            return element.Name;
        }

        protected override int Compare(Artist element1, Artist element2)
        {
            return String.Compare(element1.Name, element2.Name, StringComparison.Ordinal);
        }
    }
}