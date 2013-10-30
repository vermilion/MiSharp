using System.Collections;
using System.Collections.Generic;

namespace DeadDog.Audio.Scan.AudioScanner
{
    public partial class AudioScanner
    {
        public class ExtensionList : IList<string>
        {
            private readonly List<string> _extensions;

            public ExtensionList()
            {
                _extensions = new List<string>();
            }

            public ExtensionList(IEnumerable<string> collection)
            {
                _extensions = new List<string>();
                foreach (string s in collection)
                    Add(s);
            }

            public ExtensionList(params string[] items)
            {
                _extensions = new List<string>(items.Length);
                foreach (string s in items)
                    Add(s);
            }

            private string convert(string value)
            {
                if (value[0] != '.')
                    return "." + value;
                return value;
            }

            public override string ToString()
            {
                string s = "{";
                for (int i = 0; i < _extensions.Count; i++)
                {
                    s += _extensions[i];
                    if (i < _extensions.Count - 1)
                        s += " | ";
                }
                s += "}";
                return s;
            }

            #region IList<string> Members

            public int IndexOf(string item)
            {
                return _extensions.LastIndexOf(convert(item));
            }

            public void Insert(int index, string item)
            {
                _extensions.Insert(index, convert(item));
            }

            public void RemoveAt(int index)
            {
                _extensions.RemoveAt(index);
            }

            public string this[int index]
            {
                get { return _extensions[index]; }
                set { _extensions[index] = convert(value); }
            }

            #endregion

            #region ICollection<string> Members

            public void Add(string item)
            {
                _extensions.Add(convert(item));
            }

            public void Clear()
            {
                _extensions.Clear();
            }

            public bool Contains(string item)
            {
                return _extensions.Contains(convert(item));
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _extensions.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _extensions.Count; }
            }

            public bool IsReadOnly
            {
                get { return (_extensions as IList<string>).IsReadOnly; }
            }

            public bool Remove(string item)
            {
                return _extensions.Remove(convert(item));
            }

            public string[] ToArray()
            {
                return _extensions.ToArray();
            }

            #endregion

            #region IEnumerable<string> Members

            public IEnumerator<string> GetEnumerator()
            {
                return _extensions.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (_extensions as IEnumerable).GetEnumerator();
            }

            #endregion
        }
    }
}