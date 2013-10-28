using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DeadDog.Audio.Scan
{
    public partial class AudioScanner
    {
        public class ExistingFilesCollection : IEnumerable<RawTrack>
        {
            private readonly Comparer comparer;
            private readonly List<RawTrack> files;

            public ExistingFilesCollection()
            {
                files = new List<RawTrack>();
                comparer = new Comparer();
            }

            public int Count
            {
                get { return files.Count; }
            }

            public IEnumerator<RawTrack> GetEnumerator()
            {
                return files.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return files.GetEnumerator();
            }

            public void Add(RawTrack item)
            {
                int index = files.BinarySearch(item, comparer);
                if (~index < 0)
                    files.Add(item);
                else
                    files.Insert(~index, item);
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

                int index = files.BinarySearch(RawTrack.Unknown, new CompareToString(filepath));
                if (index >= 0)
                {
                    files.RemoveAt(index);
                    return true;
                }
                else
                    return false;
            }

            public bool Remove(RawTrack item)
            {
                return files.Remove(item);
            }

            public void Clear()
            {
                files.Clear();
            }

            public bool Contains(string filepath)
            {
                if (filepath == null)
                    return false;

                int index = files.BinarySearch(RawTrack.Unknown, new CompareToString(filepath));
                return index >= 0;
            }

            public bool Contains(RawTrack item)
            {
                return files.Contains(item);
            }

            public void CopyTo(RawTrack[] array, int arrayIndex)
            {
                files.CopyTo(array, arrayIndex);
            }

            public RawTrack[] ToArray()
            {
                return files.ToArray();
            }

            private class CompareToString : IComparer<RawTrack>
            {
                private readonly FileInfo file;

                public CompareToString(string path)
                {
                    file = new FileInfo(path);
                }

                public int Compare(RawTrack x, RawTrack y)
                {
                    return x.FullFilename.CompareTo(file.FullName);
                }
            }


            private class Comparer : IComparer<RawTrack>
            {
                public int Compare(RawTrack x, RawTrack y)
                {
                    return x.FullFilename.CompareTo(y.FullFilename);
                }
            }
        }
    }
}