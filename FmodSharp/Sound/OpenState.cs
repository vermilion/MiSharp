namespace Linsft.FmodSharp.Sound
{
    public enum OpenState : int
    {
        Ready = 0,       /* Opened and ready to play */
        Loading,         /* Initial load in progress */
        Error,           /* Failed to open - file not found, out of memory etc.  See return value of Sound::getOpenState for what happened. */
        Connecting,      /* Connecting to remote host (internet sounds only) */
        Buffering,       /* Buffering data */
        Seeking,         /* Seeking to subsound and re-flushing stream buffer. */
        Playing,         /* Ready and playing, but not possible to release at this time without stalling the main thread. */
        SetPosition,     /* Seeking within a stream to a different position. */
    }
}