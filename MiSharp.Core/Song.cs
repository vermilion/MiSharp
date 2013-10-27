using System;
using System.IO;
using MiSharp.Core.Player;
using TagLib;
using File = TagLib.File;

namespace MiSharp.Core
{
    public class Song 
    {
        private bool _isPlaying;

        public Song()
        {
            Title = string.Empty;
            Artist = "Unknown Artist";
            Album = string.Empty;
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
            Tag tag = file.GetTag(TagTypes.Id3v2);

            if (tag == null)
            {
                Title = Path.GetFileName(path);
                return;
            }

            // Title
            if (tag.Title != null)
            {
                if (tag.Title != string.Empty)
                    Title = tag.Title;
                else
                    Title = Path.GetFileName(path);
            }
            else
                Title = Path.GetFileName(path);

            // Album
            if (tag.Album != null)
                Album = tag.Album;
            else
                Album = "Unknown Album";

            // album artists
            if (tag.AlbumArtists.Length > 0)
                Artist = tag.AlbumArtists[0];
            else
                Artist = "Unknown Artist";

            // composers
            if (tag.Composers.Length > 0)
                Composers = tag.Composers;
            else
                Composers = new[] {"Unknown Artist"};

            // Genres
            if (tag.Genres.Length > 0)
                Genre = tag.Genres[0];
            else
                Genre = "Unknown Genre";

            // comments
            if (tag.Comment != string.Empty)
                Comment = tag.Comment;
            else
                Comment = string.Empty;

            // TrackNumber number
            if (tag.Track.ToString() != string.Empty)
                TrackNumber = Convert.ToInt32(tag.Track);
            else
                TrackNumber = 0;

            // Duration
            Duration = file.Properties.Duration;

            // BPM
            BeatsPerMinute = tag.BeatsPerMinute;

            // Performers
            if (tag.Performers.Length > 0)
                Performers = tag.Performers;
            else
                Performers = new[] {"Unknown Performer"};

            // Lyrics
            if (tag.Lyrics != null)
                Lyrics = tag.Lyrics;
            else
                Lyrics = string.Empty;

            // year
            if (tag.Year.ToString() != string.Empty)
                Year = tag.Year.ToString();
            else
                Year = "Unknown Year";

            if (Year == "0")
                Year = "Unknown Year";

            // date added
            DateAdded = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                //NotifyOfPropertyChange(() => IsPlaying);
            }
        }

        public AudioPlayerState State { get; set; }

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

        public string[] Performers { get; set; }

        public int Rating { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

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