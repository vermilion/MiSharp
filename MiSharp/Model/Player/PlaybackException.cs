using System;

namespace MiSharp
{
    public sealed class PlaybackException : Exception
    {
        public PlaybackException()
        {
        }

        public PlaybackException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}