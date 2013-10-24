using System;

namespace MiSharp.Core.Player
{
    public class PlaybackEventArgs : EventArgs
    {
        public int Maximum { get; set; }
        public string TotalTime { get; set; }
        public int TickFrequency { get; set; }
    }
}