using System;
using System.IO;
using MiSharp.Core.Player;
using TagLib;
using File = TagLib.File;
using Tag = TagLib.Id3v2.Tag;

namespace MiSharp.Core
{
    public class Song
    {
        public Song()
        {
            Title = string.Empty;
            Artist = "Unknown Artist";
            Album = string.Empty;
            Conductor = string.Empty;
            Year = string.Empty;
            Comment = string.Empty;
            TrackNumber = 0;
            Genre = "Unknown Genre";
            Year = string.Empty;
            DateAdded = DateTime.MinValue;
            DateUpdated = DateTime.MinValue;
        }


        public Song(string path)
        {
            OriginalPath = path;
            File file = File.Create(path);
            var id3v2 = (Tag) file.GetTag(TagTypes.Id3v2);

            if (id3v2 == null)
            {
                Title = Path.GetFileName(path);
                return;
            }

            // Title
            if (id3v2.Title != null)
            {
                if (id3v2.Title != string.Empty)
                    Title = id3v2.Title;
                else
                    Title = Path.GetFileName(path);
            }
            else
                Title = Path.GetFileName(path);

            // Album
            if (id3v2.Album != null)
                Album = id3v2.Album;
            else
                Album = "Unknown Album";

            // album artists
            if (id3v2.AlbumArtists.Length > 0)
                Artist = id3v2.AlbumArtists[0];
            else
                Artist = "Unknown Artist";

            // composers
            if (id3v2.Composers.Length > 0)
                Composers = id3v2.Composers;
            else
                Composers = new[] {"Unknown Artist"};

            // conductors
            if (id3v2.Conductor != null)
                Conductor = id3v2.Conductor;
            else
                Conductor = "Unknown Conductor";

            // Genres
            if (id3v2.Genres.Length > 0)
                Genre = id3v2.Genres[0];
            else
                Genre = "Unknown Genre";

            // comments
            if (id3v2.Comment != string.Empty)
                Comment = id3v2.Comment;
            else
                Comment = string.Empty;

            // TrackNumber number
            if (id3v2.Track.ToString() != string.Empty)
                TrackNumber = Convert.ToInt32(id3v2.Track);
            else
                TrackNumber = 0;

            // Duration
            Duration = file.Properties.Duration;

            // BPM
            BeatsPerMinute = id3v2.BeatsPerMinute;

            // Performers
            if (id3v2.Performers.Length > 0)
                Performers = id3v2.Performers;
            else
                Performers = new[] {"Unknown Performer"};

            // Lyrics
            if (id3v2.Lyrics != null)
                Lyrics = id3v2.Lyrics;
            else
                Lyrics = string.Empty;

            // year
            if (id3v2.Year.ToString() != string.Empty)
                Year = id3v2.Year.ToString();
            else
                Year = "Unknown Year";

            if (Year == "0")
                Year = "Unknown Year";

            // date added
            DateAdded = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public string OriginalPath { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Year { get; set; }

        public string Comment { get; set; }

        public int TrackNumber { get; set; }

        public string Genre { get; set; }

        public uint BeatsPerMinute { get; set; }

        public TimeSpan Duration { get; set; }

        public string Lyrics { get; set; }

        public string[] Composers { get; set; }

        public string Conductor { get; set; }

        public string[] Performers { get; set; }

        public int Rating { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        internal AudioPlayer CreateAudioPlayer()
        {
            return new LocalAudioPlayer(this);
        }

        public void WriteTag()
        {
            File file = File.Create(OriginalPath);

            if (file == null)
                return;

            // artist Song editor
            file.Tag.AlbumArtists = new[] {Artist};

            // album Song editor
            file.Tag.Album = Album;
            file.Tag.Genres = new[] {Genre};
            file.Tag.Composers = Composers;
            file.Tag.Conductor = Conductor;

            // song
            file.Tag.Title = Title;
            file.Tag.Track = Convert.ToUInt32(TrackNumber);

            uint year = 0;
            if (UInt32.TryParse(Year, out year))
                file.Tag.Year = year;

            // save
            file.Save();
        }
    }
}