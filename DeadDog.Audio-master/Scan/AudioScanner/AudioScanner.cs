using System;
using System.IO;
using System.Linq;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Parsing;

namespace DeadDog.Audio.Scan
{
    public partial class AudioScanner
    {
        private readonly DirectoryInfo _directory;

        private readonly ExtensionList _extensionList;
        private readonly AudioScan _lastScan = null;

        private readonly IDataParser _parser;

        private bool _parseAdd = true;
        private bool _parseUpdate = true;
        private bool _removeDeadFiles = true;
        private SearchOption _searchoption;

        public AudioScanner(IDataParser parser, string directory)
            : this(parser, new DirectoryInfo(directory))
        {
        }

        public AudioScanner(IDataParser parser, DirectoryInfo directory)
            : this(parser, directory, SearchOption.AllDirectories)
        {
        }

        public AudioScanner(IDataParser parser, string directory, SearchOption searchoption)
            : this(parser, new DirectoryInfo(directory), searchoption)
        {
        }

        public AudioScanner(IDataParser parser, DirectoryInfo directory, SearchOption searchoption)
            : this(parser, directory, searchoption, ".mp3", ".wma")
        {
        }

        public AudioScanner(IDataParser parser, string directory, SearchOption searchoption, params string[] extensions)
            : this(parser, new DirectoryInfo(directory), searchoption, extensions)
        {
        }

        public AudioScanner(IDataParser parser, DirectoryInfo directory, SearchOption searchoption, params string[] extensions)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (directory == null)
                throw new ArgumentNullException("directory");

            _parser = parser;
            _directory = directory;
            _searchoption = searchoption;

            _extensionList = new ExtensionList(extensions);
            MediaLibrary = new Library();
        }

        public Library MediaLibrary { get; set; }

        public string DirectoryFullName
        {
            get { return _directory.FullName; }
        }

        public string DirectoryName
        {
            get { return _directory.Name; }
        }

        public bool DirectoryExists
        {
            get
            {
                _directory.Refresh();
                return _directory.Exists;
            }
        }

        public ExtensionList Extensions
        {
            get { return _extensionList; }
        }

        public bool ParseUpdate
        {
            get { return _parseUpdate; }
            set { _parseUpdate = value; }
        }

        public bool ParseAdd
        {
            get { return _parseAdd; }
            set { _parseAdd = value; }
        }

        public bool RemoveDeadFiles
        {
            get { return _removeDeadFiles; }
            set { _removeDeadFiles = value; }
        }

        public SearchOption SearchOption
        {
            get { return _searchoption; }
            set { _searchoption = value; }
        }

        public bool IsRunning
        {
            get { return _lastScan != null && _lastScan.IsRunning; }
        }

        //public IEnumerable<RawTrack> GetExistingFiles()
        //{
        //    foreach (RawTrack rt in MediaLibrary.Tracks)
        //        yield return rt;
        //}

        public AudioScan RunScannerAsync()
        {
            return RunScannerAsync(new string[] {});
        }

        public AudioScan RunScannerAsync(string[] ignoreFiles)
        {
            if (!_directory.Exists)
                throw new ArgumentException("Directory must exist.", "directory");

            if (IsRunning)
                throw new InvalidOperationException("RunScannerAsync is already running.");

            var ig = new string[ignoreFiles.Length];
            ignoreFiles.CopyTo(ig, 0);

            ScanFileEventHandler parsed = FileParsed;
            parsed += AudioScanner_FileParsed;

            return new AudioScan(_directory, _searchoption, _parseAdd, _parseUpdate, _removeDeadFiles,
                                 _parser, _extensionList.ToArray(), MediaLibrary.Tracks.Select(x => x.Model).ToArray(), ig,
                                 parsed, ScanDone);
        }

        private void AudioScanner_FileParsed(AudioScan sender, ScanFileEventArgs e)
        {
            switch (e.State)
            {
                case FileState.Added:
                    MediaLibrary.AddTrack(e.Track);
                    break;
                case FileState.Updated:
                    MediaLibrary.RemoveTrack(e.Path);
                    MediaLibrary.AddTrack(e.Track);
                    break;
                case FileState.UpdateError:
                    MediaLibrary.RemoveTrack(e.Path);
                    break;
                case FileState.Removed:
                    MediaLibrary.RemoveTrack(e.Path);
                    break;
            }
        }

        public event ScanCompletedEventHandler ScanDone;
        public event ScanFileEventHandler FileParsed;
    }
}