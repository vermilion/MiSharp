using System;

namespace MiSharp.Core.Player.Exceptions
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