using System.Collections.Generic;

namespace MiSharp.Model.EqualityComparers
{
    public class AlbumEqualityComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return x.Album == y.Album;
        }

        public int GetHashCode(Tag obj)
        {
            return obj.Album.GetHashCode();
        }
    }
}