using System;
using System.IO;
using System.Net;
using System.Text;

namespace LyricsLibNet
{
    public abstract class HttpLyricsProviderBase : ILyricsProvider
    {
        public abstract string Name { get; }

        public void Query(string artist, string title, Action<LyricsResult> resultCallback, Action<Exception> errorCallback)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(GetRequestUrl(artist, title));
                request.BeginGetResponse(ResponseCallback, new State
                    {
                        Artist = artist,
                        Title = title,
                        Request = request,
                        SuccessCallback = resultCallback,
                        ErrorCallback = errorCallback
                    });
            }
            catch (Exception ex)
            {
                errorCallback(ex);
            }
        }

        public abstract string GetRequestUrl(string artist, string title);
        public abstract LyricsResult ParseResult(string requestArtist, string requestTrackTitle, string result);

        private void ResponseCallback(IAsyncResult result)
        {
            var state = (State) result.AsyncState;

            try
            {
                var response = (HttpWebResponse) state.Request.EndGetResponse(result);
                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string resultString = reader.ReadToEnd();
                        state.SuccessCallback(ParseResult(state.Artist, state.Title, resultString));
                    }
                }
            }
            catch (Exception ex)
            {
                state.ErrorCallback(ex);
            }
        }

        private class State
        {
            public string Artist { get; set; }
            public string Title { get; set; }
            public HttpWebRequest Request { get; set; }
            public Action<LyricsResult> SuccessCallback { get; set; }
            public Action<Exception> ErrorCallback { get; set; }
        }
    }
}