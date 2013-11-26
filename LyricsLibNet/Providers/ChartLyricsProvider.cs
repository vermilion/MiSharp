using System;
using System.Xml.Linq;

namespace LyricsLibNet.Providers
{
    public class ChartLyricsProvider : HttpLyricsProviderBase
    {
        public override string Name
        {
            get { return "chartlyrics.com"; }
        }

        public override string GetRequestUrl(string artist, string title)
        {
            return "http://api.chartlyrics.com/apiv1.asmx/SearchLyricDirect?artist=" + Uri.EscapeDataString(artist) + "&song=" + Uri.EscapeDataString(title);
        }

        public override LyricsResult ParseResult(string requestArtist, string requestTrackTitle, string result)
        {
            XNamespace xmlSpace = XNamespace.Get("http://api.chartlyrics.com/");

            XDocument document = XDocument.Parse(result);
            XElement root = document.Root;
            string artist = root.Element(xmlSpace.GetName("LyricArtist")).Value.Trim();
            string song = root.Element(xmlSpace.GetName("LyricSong")).Value.Trim();
            string text = root.Element(xmlSpace.GetName("Lyric")).Value.Trim();

            var lyricsResult = new LyricsResult(Name, artist, song, text);

            XElement url = root.Element(xmlSpace.GetName("LyricUrl"));
            if (url != null)
            {
                lyricsResult.AdditionalFields["Url"] = url.Value.Trim();
            }
            XElement rank = root.Element(xmlSpace.GetName("LyricRank"));
            if (rank != null)
            {
                lyricsResult.AdditionalFields["Rank"] = rank.Value.Trim();
            }
            XElement covertArtUrl = root.Element(xmlSpace.GetName("LyricCovertArtUrl"));
            if (rank != null)
            {
                lyricsResult.AdditionalFields["CovertArtUrl"] = covertArtUrl.Value.Trim();
            }

            return lyricsResult;
        }
    }
}