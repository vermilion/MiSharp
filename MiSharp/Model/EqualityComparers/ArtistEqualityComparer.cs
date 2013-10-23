using System.Collections.Generic;

namespace MiSharp.Model.EqualityComparers
{
    public class ArtistEqualityComparer : IEqualityComparer<Song>
    {
        public bool Equals(Song x, Song y)
        {
            return x.Artist == y.Artist;
        }

        public int GetHashCode(Song obj)
        {
            return obj.Artist.GetHashCode();
        }
    }
}