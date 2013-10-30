using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DeadDog.Audio.Scan.AudioScanner
{
    public partial class AudioScanner
    {
        public class ExistingFilesCollection : IEnumerable<RawTrack>
        {
            private readonly Comparer _comparer;
            private readonly List<RawTrack> _files;

            public ExistingFilesCollection()
            {
                _files = new List<RawTrack>();
                _comparer = new Comparer();
            }

            public int Count
            {
                get { return _files.Count; }
            }

            public IEnumerator<RawTrack> GetEnumerator()
            {
                return _files.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _files.GetEnumerator();
            }

            public void Add(RawTrack item)
            {
                int index = _files.BinarySearch(item, _comparer);
                if (~index < 0)
                    _files.Add(item);
                else
                    _files.Insert(~index, item);
            }

            public void AddRange(IEnumerable<RawTrack> collection)
            {
                foreach (RawTrack rt in collection)
                    Add(rt);
            }

            public bool Remove(string filepath)
            {
                if (filepath == null)
                    return false;

                int index = _files.BinarySearch(RawTrack.Unknown, new CompareToString(filepath));
                if (index >= 0)
                {
                    _files.RemoveAt(index);
                    return true;
                }
                return false;
            }

            public bool Remove(RawTrack item)
            {
                return _files.Remove(item);
            }

            public void Clear()
            {
                _files.Clear();
            }

            public bool Contains(string filepath)
            {
                if (filepath == null)
                    return false;

                int index = _files.BinarySearch(RawTrack.Unknown, new CompareToString(filepath));
                return index >= 0;
            }

            public bool Contains(RawTrack item)
            {
                return _files.Contains(item);
            }

            public void CopyTo(RawTrack[] array, int arrayIndex)
            {
                _files.CopyTo(array, arrayIndex);
            }

            public RawTrack[] ToArray()
            {
                return _files.ToArray();
            }

            private class CompareToString : IComparer<RawTrack>
            {
                private readonly FileInfo _file;

                public CompareToString(string path)
                {
                    _file = new FileInfo(path);
                }

                public int Compare(RawTrack x, RawTrack y)
                {
                    return String.Compare(x.FullFilename, _file.FullName, StringComparison.Ordinal);
                }
            }


            private class Comparer : IComparer<RawTrack>
            {
                public int Compare(RawTrack x, RawTrack y)
                {
                    return String.Compare(x.FullFilename, y.FullFilename, StringComparison.Ordinal);
                }
            }
        }
    }
}