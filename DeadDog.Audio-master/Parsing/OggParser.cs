using Luminescence.Xiph;

namespace DeadDog.Audio.Parsing
{
    public class OggParser : IDataParser
    {
        public RawTrack ParseTrack(string filepath)
        {
            var ogg = new OggTagger(filepath);
            int trackNumber;
            if (!int.TryParse(ogg.TrackNumber, out trackNumber))
                trackNumber = RawTrack.TrackNumberIfUnknown;

            int year;
            if (!int.TryParse(ogg.Date, out year))
                year = RawTrack.YearIfUnknown;

            return new RawTrack(filepath, ogg.Title, ogg.Album, trackNumber, ogg.Artist, year);
        }
    }
}