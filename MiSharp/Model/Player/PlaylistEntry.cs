using System;
using MiSharp.Model;
using Rareform.Validation;

namespace MiSharp
{
    public class PlaylistEntry : IComparable<PlaylistEntry>
    {
        internal PlaylistEntry(int index, Song song)
        {
            if (index < 0)
                Throw.ArgumentOutOfRangeException(() => index, 0);

            if (song == null)
                Throw.ArgumentNullException(() => song);

            Index = index;
            Song = song;
        }

        public int Index { get; internal set; }

        public Song Song { get; private set; }

        public int CompareTo(PlaylistEntry other)
        {
            return Index.CompareTo(other.Index);
        }
    }
}