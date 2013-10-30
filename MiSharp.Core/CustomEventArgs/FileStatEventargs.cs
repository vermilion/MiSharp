using System;
using System.IO;

namespace MiSharp.Core.CustomEventArgs
{
    public class FileStatEventArgs : EventArgs
    {
        public FileInfo File { get; set; }
        public Int64 CurrentFileNumber { get; set; }
        public Int64 TotalFiles { get; set; }

        public bool Completed
        {
            get { return CurrentFileNumber == TotalFiles; }
        }
    }
}