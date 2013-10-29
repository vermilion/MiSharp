using System;

namespace DeadDog.Audio.Libraries.Collections
{
    public class AlbumCollection : LibraryCollectionBase<Album>
    {
        internal AlbumCollection()
        {
        }

        public event AlbumEventHandler AlbumAdded , AlbumRemoved;

        protected override void OnAdded(Album element)
        {
            if (AlbumAdded != null)
                AlbumAdded(this, new AlbumEventArgs(element));
        }

        protected override void OnRemoved(Album element)
        {
            if (AlbumRemoved != null)
                AlbumRemoved(this, new AlbumEventArgs(element));
        }

        protected override string GetName(Album element)
        {
            return element.Title;
        }

        protected override int Compare(Album element1, Album element2)
        {
            return String.Compare(element1.Title, element2.Title, StringComparison.Ordinal);
        }
    }
}