using System;

namespace MiSharp.Model.Playlist
{
    public class TrackbarEventArgs : EventArgs
    {
        public int Position { get; set; }
        public string CurrentTime { get; set; }
    }
}