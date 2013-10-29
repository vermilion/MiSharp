using System;
using System.Globalization;
using System.IO;
using System.Linq;
using TagLib;

namespace DeadDog.Audio
{
    public class Mp3Track : RawTrack
    {
        private readonly string _filepath;

        public Mp3Track(string filepath, Tag tag)
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

            TrackTitle = string.IsNullOrEmpty(tag.Title) ? File.Name : tag.Title.Trim();
            AlbumTitle = string.IsNullOrEmpty(tag.Album) ? null : tag.Album.Trim();
            TrackNumber = (int) tag.Track;
            ArtistName = !tag.Performers.Any() ? null : string.Join(" & ", tag.Performers);
            Year = (int) tag.Year;
            Genre = tag.JoinedGenres;
        }

        public void WriteTag()
        {
            var file = TagLib.File.Create(_filepath);

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