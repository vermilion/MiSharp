using System;

namespace DeadDog.Audio.Scan
{
    /// <summary>
    ///     Provides data for file-events associated with the <see cref="AudioScan" /> class.
    /// </summary>
    public class ScanFileEventArgs : EventArgs
    {
        private readonly string filepath;
        private readonly FileState filestate;
        private readonly RawTrack track;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScanFileEventArgs" /> class.
        /// </summary>
        /// <param name="filepath">The file path affected by the event.</param>
        /// <param name="track">The track (if one exists) parsed from the file affected by the event.</param>
        /// <param name="filestate">The file state affected by the event.</param>
        public ScanFileEventArgs(string filepath, RawTrack track, FileState filestate)
        {
            this.filepath = filepath;
            this.track = track;
            this.filestate = filestate;
        }

        /// <summary>
        ///     Gets the track (if one exists) parsed from the file affected by the event.
        /// </summary>
        public RawTrack Track
        {
            get { return track; }
        }

        /// <summary>
        ///     Gets the path to the file affected by the event.
        /// </summary>
        public string Path
        {
            get { return filepath; }
        }

        /// <summary>
        ///     Gets the state of the file affected by the event.
        /// </summary>
        public FileState State
        {
            get { return filestate; }
        }
    }
}