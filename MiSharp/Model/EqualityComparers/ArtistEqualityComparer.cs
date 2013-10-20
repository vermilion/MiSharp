using System.Collections.Generic;

namespace MiSharp.Model.EqualityComparers
{
    public class ArtistEqualityComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return x.AlbumArtist == y.AlbumArtist;
        }

        public int GetHashCode(Tag obj)
        {
            return obj.AlbumArtist.GetHashCode();
        }
    }
}