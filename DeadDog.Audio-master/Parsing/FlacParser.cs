using Luminescence.Xiph;

namespace DeadDog.Audio.Parsing
{
    public class FlacParser : IDataParser
    {
        public RawTrack ParseTrack(string filepath)
        {
            var flac = new FlacTagger(filepath);
            int trackNumber;
            if (!int.TryParse(flac.TrackNumber, out trackNumber))
                trackNumber = -1;

            int year;
            if (!int.TryParse(flac.Date, out year))
                year = RawTrack.YearIfUnknown;

            return new RawTrack(filepath, flac.Title, flac.Album, trackNumber, flac.Artist, year);
        }
    }
}