using System;

namespace LyricsLibNet
{
    public class ResultAvailableEventArgs : EventArgs
    {
        public ResultAvailableEventArgs(bool isLast, LyricsResult result)
        {
            IsLast = isLast;
            Result = result;
        }

        public bool IsLast { get; private set; }
        public LyricsResult Result { get; private set; }
    }
}