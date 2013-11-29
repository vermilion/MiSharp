using System;

namespace LyricsLibNet
{
    internal static class StringHelper
    {
        public static string GetTextBetween(string source, string start, string end)
        {
            int startIndex = source.IndexOf(start, StringComparison.Ordinal);
            int endIndex = source.IndexOf(end, startIndex, StringComparison.Ordinal);

            if (startIndex == -1 || endIndex == -1)
            {
                throw new InvalidOperationException("Start or end text not found.");
            }

            return source.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
        }
    }
}