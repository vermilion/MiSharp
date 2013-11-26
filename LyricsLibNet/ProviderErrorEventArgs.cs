using System;

namespace LyricsLibNet
{
    public class ProviderErrorEventArgs : EventArgs
    {
        public ProviderErrorEventArgs(bool isLast, string providerName, Exception exception)
        {
            IsLast = isLast;
            ProviderName = providerName;
            Exception = exception;
        }

        public bool IsLast { get; private set; }
        public string ProviderName { get; private set; }
        public Exception Exception { get; private set; }
    }
}