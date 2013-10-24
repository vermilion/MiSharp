using System;

namespace MiSharp.Core.Player.Exceptions
{
    public class SongLoadException : Exception
    {
        public SongLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SongLoadException()
        {
        }
    }
}