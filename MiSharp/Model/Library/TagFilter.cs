namespace MiSharp.Model.Library
{
    public class TagFilter
    {
        public TagFilter(string bandName, string albumName)
        {
            BandName = bandName;
            AlbumName = albumName;
        }

        public string BandName { get; set; }
        public string AlbumName { get; set; }
    }
}