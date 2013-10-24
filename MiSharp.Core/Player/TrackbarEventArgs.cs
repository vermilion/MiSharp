using System;

namespace MiSharp.Core.Player
{
    public class TrackbarEventArgs : EventArgs
    {
        public int Position { get; set; }
        public string CurrentTime { get; set; }
    }
}