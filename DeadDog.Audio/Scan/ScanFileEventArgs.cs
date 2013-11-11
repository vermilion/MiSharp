using System;
using DeadDog.Audio.Scan.AudioScanner;

namespace DeadDog.Audio.Scan
{
    /// <summary>
    ///     Provides data for file-events associated with the <see cref="AudioScan" /> class.
    /// </summary>
    public class ScanFileEventArgs : EventArgs
    {
        private readonly string _filepath;
        private readonly FileState _filestate;
        private readonly RawTrack _track;
        private readonly long _currentFileNumber;
        private readonly long _totalFilesCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScanFileEventArgs" /> class.
        /// </summary>
        /// <param name="filepath">The file path affected by the event.</param>
        /// <param name="track">The track (if one exists) parsed from the file affected by the event.</param>
        /// <param name="filestate">The file state affected by the event.</param>
        public ScanFileEventArgs(long currentFileNumber, long totalFilesCount, string filepath, RawTrack track, FileState filestate)
        {
            _currentFileNumber = currentFileNumber;
            _totalFilesCount = totalFilesCount;
            _filepath = filepath;
            _track = track;
            _filestate = filestate;
        }

        /// <summary>
        ///     Gets the track (if one exists) parsed from the file affected by the event.
        /// </summary>
        public RawTrack Track
        {
            get { return _track; }
        }

        /// <summary>
        ///     Gets the path to the file affected by the event.
        /// </summary>
        public string Path
        {
            get { return _filepath; }
        }

        /// <summary>
        ///     Gets the state of the file affected by the event.
        /// </summary>
        public FileState State
        {
            get { return _filestate; }
        }

        public long CurrentFileNumber
        {
            get { return _currentFileNumber; }
        }

        public long TotalFilesCount
        {
            get { return _totalFilesCount; }
        }
    }
}