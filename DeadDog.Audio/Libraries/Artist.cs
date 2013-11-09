using System;
using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries
{
    public class Artist
    {
        #region Properties

        public Guid Identifier { get; set; }

        public string Name { get; set; }

        public AlbumCollection Albums { get; set; }

        #endregion

        public Artist(string name)
        {
            Albums = new AlbumCollection();
            Identifier = Guid.NewGuid();

            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}