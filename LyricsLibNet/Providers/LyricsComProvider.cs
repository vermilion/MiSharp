using System;
using System.Text;

namespace LyricsLibNet.Providers
{
    public class LyricsComProvider : HttpLyricsProviderBase
    {
        public override string Name
        {
            get { return "lyrics.com"; }
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
                else if (char.IsWhiteSpace(c))
                {
                    builder.Append("-");
                }
            }
            return builder.ToString();
        }

        public override string GetRequestUrl(string artist, string title)
        {
            return "http://www.lyrics.com/" + FixName(title) + "-lyrics-" + FixName(artist) + ".html";
        }

        public override LyricsResult ParseResult(string requestArtist, string requestTrackTitle, string result)
        {
            string profileName = StringHelper.GetTextBetween(result, "<h1 id=\"profile_name\">", "</h1>");
            string artist = StringHelper.GetTextBetween(StringHelper.GetTextBetween(profileName, "<a href=", "</span>"), "\">", "</a>");
            string title = profileName.Substring(0, profileName.IndexOf("<br />", StringComparison.Ordinal));
            string lyrics = StringHelper.GetTextBetween(result, "<div id=\"lyric_space\">", "<br />---<br />").Trim().Replace("<br />", "");

            return new LyricsResult(Name, artist, title, lyrics);
        }
    }
}