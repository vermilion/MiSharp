using System;
using System.IO;

namespace MiSharp.Core.Library
{
    public class FileStatEventargs : EventArgs
    {
        public FileInfo File { get; set; }
        public Int64 CurrentFileNumber { get; set; }
        public Int64 TotalFiles { get; set; }
    }
}