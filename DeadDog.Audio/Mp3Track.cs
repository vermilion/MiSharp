using System;
using System.IO;
using System.Linq;
using TagLib;

namespace DeadDog.Audio
{
    public class Mp3Track : RawTrack
    {
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

            _track = string.IsNullOrEmpty(tag.Title) ? null : tag.Title.Trim();
            _album = string.IsNullOrEmpty(tag.Album) ? null : tag.Album.Trim();
            _number = (int) tag.Track;
            _artist = !tag.Performers.Any() ? null : string.Join(" & ", tag.Performers);
            _year = (int) tag.Year;
        }
    }
}