using System;
using DeadDog.Audio.Libraries.Collections;
using DeadDog.Audio.Libraries.Events;

namespace DeadDog.Audio.Libraries
{
    public class Album
    {
        #region Properties

        public Guid Identifier { get; set; }

        public string Title { get; set; }

        public TrackCollection Tracks { get; set; }

        public Artist Artist { get; set; }

        public int Year { get; set; }

        public bool HasArtist
        {
            get { return Artist != null; }
        }

        #endregion

        public Album(string album, int albumYear)
        {
            Year = albumYear;
            Tracks = new TrackCollection(TrackAdded, TrackRemoved);
            Identifier = Guid.NewGuid();

            Title = album;
        }

        public override string ToString()
        {
            return Title;
        }

        private void TrackAdded(TrackCollection collection, TrackEventArgs e)
        {
            if (collection != Tracks)
                throw new InvalidOperationException("Album attempted to alter wrong trackcollection.");

            if (collection.Count == 1)
            {
                Artist = e.Track.Artist;
                Artist.Albums.Add(this);
            }
            else if (e.Track.Artist != null && e.Track.Artist != Artist)
            {
                if (Artist != null)
                    Artist.Albums.Remove(this);
                Artist = null;
            }
        }

        private void TrackRemoved(TrackCollection collection, TrackEventArgs e)
        {
            if (collection != Tracks)
                throw new InvalidOperationException("Album attempted to alter wrong trackcollection.");

            Artist temp = null;
            for (int i = 0; i < collection.Count; i++)
                if (temp == null)
                    temp = collection[i].Artist;
                else if (collection[i].Artist != null && collection[i].Artist != temp)
                {
                    if (Artist != null)
                        Artist.Albums.Remove(this);
                    Artist = null;
                    return;
                }

            // All track artist are the same (or null)
            if (Artist != null)
                Artist.Albums.Remove(this);

            Artist = temp;

            if (temp != null)
                Artist.Albums.Add(this);
        }
    }
}