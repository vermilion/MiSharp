using System.Collections.Generic;

namespace MiSharp.Model.EqualityComparers
{
    public class AlbumEqualityComparer : IEqualityComparer<Song>
    {
        public bool Equals(Song x, Song y)
        {
            return x.Album == y.Album;
        }

        public int GetHashCode(Song obj)
        {
            return obj.Album.GetHashCode();
        }
    }
}