using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DeadDog.Audio.Parsing;

namespace DeadDog.Audio.Scan.AudioScanner
{
    public class AudioScan
    {
        private readonly DirectoryInfo _directory;
        private readonly Dictionary<FileInfo, RawTrack> _existingFiles;
        private readonly string[] _extensions;
        private readonly FileInfo[] _ignoredFiles;

        private readonly bool _parseAdd;
        private readonly bool _parseUpdate;

        private readonly IDataParser _parser;
        private readonly bool _removeDeadFiles;
        private readonly SearchOption _searchoption;

        private int _added;
        private int _error;
        private int _removed;
        private int _skipped;
        private ScannerState _state;
        private int _total;
        private int _updated;

        internal AudioScan(DirectoryInfo directory, SearchOption searchoption,
                           bool parseAdd, bool parseUpdate, bool removeDeadFiles,
                           string[] extensions, IEnumerable<RawTrack> existingFiles, IEnumerable<string> ignoredFiles,
                           ScanFileEventHandler parsed,
                           ScanCompletedEventHandler done)
        {
            var thread = new Thread(Run);

            _directory = directory;
            _searchoption = searchoption;

            _parseAdd = parseAdd;
            _parseUpdate = parseUpdate;
            _removeDeadFiles = removeDeadFiles;

            _parser = new MediaParser();

            _extensions = extensions;
            _existingFiles = existingFiles.ToDictionary(rt => rt.File);

            _ignoredFiles = (from s in ignoredFiles select new FileInfo(s)).ToArray();

            Parsed = parsed;
            Done = done;

            _state = ScannerState.NotRunning;

            _added = _updated = _skipped = _error = _removed = _total = 0;

            thread.Start();
        }

        #region Properties

        public DirectoryInfo Directory
        {
            get { return _directory; }
        }

        public SearchOption SearchOption
        {
            get { return _searchoption; }
        }

        public bool ParseAdd
        {
            get { return _parseAdd; }
        }

        public bool ParseUpdate
        {
            get { return _parseUpdate; }
        }

        public bool RemoveDeadFiles
        {
            get { return _removeDeadFiles; }
        }

        public string[] FileExtensions
        {
            get { return _extensions; }
        }

        public ScannerState State
        {
            get { return _state; }
        }

        public int Added
        {
            get { return _added; }
        }

        public int Updated
        {
            get { return _updated; }
        }

        public int Skipped
        {
            get { return _skipped; }
        }

        public int Errors
        {
            get { return _error; }
        }

        public int Removed
        {
            get { return _removed; }
        }

        public int Total
        {
            get { return _total; }
        }

        public bool IsRunning
        {
            get { return _state != ScannerState.Completed && _state != ScannerState.NotRunning; }
        }

        #endregion

        private event ScanFileEventHandler Parsed;
        private event ScanCompletedEventHandler Done;

        private void Run()
        {
            _state = ScannerState.Scanning;

            Dictionary<FileInfo, Action> actions = BuildActionDictionary(ScanForFiles(), _existingFiles.Keys);
            foreach (FileInfo file in _ignoredFiles)
                actions[file] = Action.Skip;

            _total = actions.Count;

            _state = ScannerState.Parsing;

            foreach (var file in actions)
                ParseFile(file.Key, file.Value);

            _state = ScannerState.Completed;
            if (Done != null)
                Done(this, new ScanCompletedEventArgs());
        }

        private IEnumerable<FileInfo> ScanForFiles()
        {
            //Get all files in directory
            var searchFiles = new List<FileInfo>();
            foreach (string ext in _extensions)
            {
                FileInfo[] files = _directory.GetFiles("*" + ext, _searchoption);
                searchFiles.AddRange(files);
            }

            searchFiles = new List<FileInfo>(TrimForExtensions(searchFiles));

            return searchFiles;
        }

        private IEnumerable<FileInfo> TrimForExtensions(IEnumerable<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                bool ok = false;
                foreach (string ext in _extensions)
                    if (file.Extension.ToLower() == ext)
                    {
                        ok = true;
                        break;
                    }
                if (ok)
                    yield return file;
            }
        }

        private Dictionary<FileInfo, Action> BuildActionDictionary(IEnumerable<FileInfo> scanFiles, IEnumerable<FileInfo> existingFiles)
        {
            var dictionary = new Dictionary<FileInfo, Action>();

            if (!_parseAdd && !_parseUpdate && !_removeDeadFiles)
                return existingFiles.ToDictionary(x => x, x => Action.Skip);

            var scan = new List<FileInfo>(scanFiles);
            scan.Sort(ComparePath);

            var exist = new List<FileInfo>(existingFiles);
            exist.Sort(ComparePath);

            int s = 0;
            int e = 0;
            while (s < scan.Count || s < exist.Count)
            {
                int compare = s == scan.Count ? 1 : e == exist.Count ? -1 : ComparePath(scan[s], exist[e]);
                if (compare < 0)
                {
                    if (_parseAdd)
                        dictionary.Add(scan[s], Action.Add);
                    s++;
                }
                else if (compare > 0)
                {
                    if (_removeDeadFiles)
                        dictionary.Add(exist[e], Action.Remove);
                    else
                        dictionary.Add(exist[e], Action.Skip);
                    e++;
                }
                else if (compare == 0)
                {
                    if (_parseUpdate)
                        dictionary.Add(exist[e], Action.Update);
                    else
                        dictionary.Add(exist[e], Action.Skip);
                    s++;
                    e++;
                }
            }

            return dictionary;
        }

        private void ParseFile(FileInfo file, Action action)
        {
            switch (action)
            {
                case Action.Add:
                case Action.Update:
                    RawTrack rt;
                    if (_parser.TryParseTrack(file.FullName, out rt))
                    {
                        if (action == Action.Add)
                            FileParsed(file, rt, FileState.Added);
                        else if (_existingFiles[file].Equals(rt))
                            FileParsed(file, FileState.Skipped);
                        else
                            FileParsed(file, rt, FileState.Updated);
                    }
                    else if (action == Action.Update && IsFileLocked(file))
                        FileParsed(file, FileState.Skipped);
                    else
                        FileParsed(file, action == Action.Add ? FileState.AddError : FileState.UpdateError);
                    break;
                case Action.Skip:
                    FileParsed(file, FileState.Skipped);
                    break;
                case Action.Remove:
                    FileParsed(file, FileState.Removed);
                    break;
                default:
                    throw new InvalidOperationException("Unknown file action.");
            }
        }

        private void FileParsed(FileInfo filepath, FileState state)
        {
            RawTrack track = null;
            _existingFiles.TryGetValue(filepath, out track);
            FileParsed(filepath, track, state);
        }

        private void FileParsed(FileInfo filepath, RawTrack track, FileState state)
        {
            switch (state)
            {
                case FileState.Added:
                    _added++;
                    break;
                case FileState.Updated:
                    _updated++;
                    break;
                case FileState.AddError:
                case FileState.UpdateError:
                case FileState.Error:
                    _error++;
                    break;
                case FileState.Removed:
                    _removed++;
                    break;
                case FileState.Skipped:
                    _skipped++;
                    break;
                default:
                    throw new InvalidOperationException("Unknown filestate.");
            }

            if (Parsed != null)
                Parsed(this, new ScanFileEventArgs(filepath.FullName, track, state));
        }

        private bool IsFileLocked(FileInfo file)
        {
            file.Refresh();
            if (!file.Exists)
                return false;

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        #region Path comparison

        private int ComparePath(RawTrack x, RawTrack y)
        {
            return String.Compare(x.FullFilename, y.FullFilename, StringComparison.Ordinal);
        }

        private int ComparePath(FileInfo x, FileInfo y)
        {
            return String.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
        }

        private int ComparePath(RawTrack x, FileInfo y)
        {
            return String.Compare(x.FullFilename, y.FullName, StringComparison.Ordinal);
        }

        private int ComparePath(FileInfo x, RawTrack y)
        {
            return String.Compare(x.FullName, y.FullFilename, StringComparison.Ordinal);
        }

        private bool PathEqual(RawTrack x, RawTrack y)
        {
            return x.FullFilename.Equals(y.FullFilename);
        }

        private bool PathEqual(FileInfo x, FileInfo y)
        {
            return x.FullName.Equals(y.FullName);
        }

        private bool PathEqual(RawTrack x, FileInfo y)
        {
            return x.FullFilename.Equals(y.FullName);
        }

        private bool PathEqual(FileInfo x, RawTrack y)
        {
            return x.FullName.Equals(y.FullFilename);
        }

        #endregion

        private enum Action
        {
            Add,
            Update,
            Skip,
            Remove
        }
    }
}