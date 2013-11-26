using System;
using System.Text;

namespace LyricsLibNet.Providers
{
    public class AzLyricsProvider : HttpLyricsProviderBase
    {
        public override string Name
        {
            get { return "azlyrics.com"; }
        }

        private string FixName(string name)
        {
            var builder = new StringBuilder();
            foreach (char c in name)
            {
                if (char.IsLetter(c))
                {
                    builder.Append(char.ToLower(c));
                }
            }
            return builder.ToString();
        }

        public override string GetRequestUrl(string artist, string title)
        {
            return "http://www.azlyrics.com/lyrics/" + FixName(artist) + "/" + FixName(title) + ".html";
        }

        public override LyricsResult ParseResult(string requestArtist, string requestTrackTitle, string result)
        {
            string artist = StringHelper.GetTextBetween(result, "<h2>", "</h2>");
            if (!artist.EndsWith(" LYRICS")) throw new Exception();
            artist = artist.Substring(0, artist.Length - " LYRICS".Length);

            string title = StringHelper.GetTextBetween(result, "<b>", "</b>").Trim('"');

            string lyrics = StringHelper.GetTextBetween(result, "<!-- start of lyrics -->", "<!-- end of lyrics -->").Trim();
            lyrics = lyrics.Replace("<br />", "");

            var lyricsResult = new LyricsResult(Name, artist, title, lyrics);
            return lyricsResult;
        }
    }
}