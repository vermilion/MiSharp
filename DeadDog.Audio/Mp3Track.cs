using System;
using System.Globalization;
using System.IO;
using System.Linq;
using File = TagLib.File;

namespace DeadDog.Audio
{
    public class Mp3Track : RawTrack
    {
        private readonly string _filepath;

        public Mp3Track(string filepath, File tag)
        {
            if (filepath == null)
                throw new ArgumentNullException("filepath", "filepath cannot equal null");
            try
            {
                File = new FileInfo(filepath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured from the passed filepath", "filepath", e);
            }
            _filepath = filepath;

            TrackTitle = string.IsNullOrEmpty(tag.Tag.Title) ? File.Name : tag.Tag.Title.Trim();
            AlbumTitle = string.IsNullOrEmpty(tag.Tag.Album) ? null : tag.Tag.Album.Trim();
            TrackNumber = (int) tag.Tag.Track;
            ArtistName = !tag.Tag.Performers.Any() ? null : string.Join(" & ", tag.Tag.Performers);
            Year = (int) tag.Tag.Year;
            Genre = tag.Tag.JoinedGenres;
            Bitrate = tag.Tag.BeatsPerMinute;
            Duration = tag.Properties.Duration;
        }

        public void WriteTag()
        {
            File file = TagLib.File.Create(_filepath);

            if (file == null)
                return;

            // artist tag editor
            file.Tag.AlbumArtists = new[] {ArtistName};

            // album tag editor
            file.Tag.Album = AlbumTitle;
            file.Tag.Genres = new[] {Genre};
            //file.Tag.Composers = Composers;
            //file.Tag.Conductor = Conductor;
            uint year;
            if (UInt32.TryParse(Year.ToString(CultureInfo.InvariantCulture), out year))
                file.Tag.Year = year;

            // song
            file.Tag.Title = TrackTitle;
            file.Tag.Track = Convert.ToUInt32(TrackNumber);

            // save
            file.Save();
        }
    }
}