using System.Collections.Generic;

namespace LyricsLibNet
{
    public class LyricsResult
    {
        public LyricsResult(string providerName, string artist, string trackTitle, string text)
        {
            ProviderName = providerName;
            Artist = artist;
            TrackTitle = trackTitle;
            Text = text;
            AdditionalFields = new Dictionary<string, string>();
        }

        public string ProviderName { get; set; }
        public string Artist { get; set; }
        public string TrackTitle { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> AdditionalFields { get; private set; }
    }
}