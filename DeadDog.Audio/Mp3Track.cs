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
                _file = new FileInfo(filepath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured from the passed filepath", "filepath", e);
            }
            _filepath = filepath;

            _track = string.IsNullOrEmpty(tag.Title) ? null : tag.Title.Trim();
            _album = string.IsNullOrEmpty(tag.Album) ? null : tag.Album.Trim();
            _number = (int) tag.Track;
            _artist = !tag.Performers.Any() ? null : string.Join(" & ", tag.Performers);
            _year = (int) tag.Year;
            _genre = tag.JoinedGenres;
        }

                public void WriteTag()
        {
            TagLib.File file = TagLib.File.Create(_filepath);

            if (file == null)
                return;

            // artist tag editor
            file.Tag.AlbumArtists = new[] {ArtistName};

            // album tag editor
            file.Tag.Album = AlbumTitle;
            file.Tag.Genres = new[] {Genre};
            //file.Tag.Composers = Composers;
            //file.Tag.Conductor = Conductor;
            uint year = 0;
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