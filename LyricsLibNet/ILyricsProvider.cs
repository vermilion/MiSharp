using System;

namespace LyricsLibNet
{
    public interface ILyricsProvider
    {
        string Name { get; }
        void Query(string requestArtist, string requestTrackTitle, Action<LyricsResult> resultCallback, Action<Exception> errorCallback);
    }
}