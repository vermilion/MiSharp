using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Text;
using Caliburn.Micro;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class LyricsViewModel : Screen
    {
        private string _lyricsText;

        public LyricsViewModel()
        {
            var playerViewModel = IoC.Get<PlayerViewModel>();
            

            GetLyricsCommand = new ReactiveCommand();
            GetLyricsCommand.Subscribe(x =>
                {
                    var current = playerViewModel.CurrentlyPlaying;
                    if (current != null)
                        LyricsText = Getlyrics(current.Model.ArtistName, current.Model.TrackTitle);
                });
        }

        public ReactiveCommand GetLyricsCommand { get; set; }

        public string LyricsText
        {
            get { return _lyricsText; }
            set
            {
                _lyricsText = value;
                NotifyOfPropertyChange(() => LyricsText);
            }
        }

        private string Getlyrics(string strArtist, string strSongTitle)
        {
            var wc = new WebClient();
            string sUrl = @"http://lyrics.wikia.com/index.php?title=" + Format(strArtist) + ":" + Format(strSongTitle) + "&action=edit";

            //Set encoding to UTF8 to handle accented characters.
            wc.Encoding = Encoding.UTF8;
            string sLyrics = wc.DownloadString(sUrl);

            //Get surrounding tags.
            int iStart = sLyrics.IndexOf("&lt;lyrics>", StringComparison.Ordinal) + 12;
            int iEnd = sLyrics.IndexOf("&lt;/lyrics>", StringComparison.Ordinal) - 1;

            //Replace webpage standard newline feed with carriage return + newline feed, which is standard on Windows.
            sLyrics = Edit(sLyrics, iStart, iEnd).Replace("\n", Environment.NewLine).TrimEnd();

            //If Lyrics Wikia is suggesting a redirect, pull lyrics for that.
            if (sLyrics.Contains("#REDIRECT"))
            {
                iStart = sLyrics.IndexOf("#REDIRECT [[", StringComparison.Ordinal) + 12;
                iEnd = sLyrics.IndexOf("]]", iStart, StringComparison.Ordinal);
                strArtist = Edit(sLyrics, iStart, iEnd).Split(':')[0];
                strSongTitle = Edit(sLyrics, iStart, iEnd).Split(':')[1];
                Getlyrics(strArtist, strSongTitle);
            }

            //If lyrics weren't found :-(
            else if (sLyrics.Contains("!-- PUT LYRICS HERE (and delete this entire line) -->"))
                sLyrics = "Lyrics not found.";

            return sLyrics;
        }

        //Substring method, but with starting index and ending index too.
        private static string Edit(string source, int start, int end)
        {
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;
            return source.Substring(start, len);
        }

        //Method replaces first letter of all words to UPPERCASE and replaces all spaces with underscores.
        private static string Format(string s)
        {
            char[] array = s.Trim().ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array).Trim().Replace(' ', '_');
        }
    }
}