using System;

namespace MiSharp.Core.Player
{
    public class PlaybackEventArgs : EventArgs
    {
        public PlaybackEventArgs()
        {
            CurrentTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            PlaybackState = AudioPlayerState.None;
        }

        public TimeSpan CurrentTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public AudioPlayerState PlaybackState { get; set; }
        public float Volume { get; set; }
    }
}