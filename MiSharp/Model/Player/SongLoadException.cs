using System;

namespace MiSharp
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