using System.Collections;
using System.Collections.Generic;

namespace DeadDog.Audio.Scan
{
    public partial class AudioScanner
    {
        public class ExtensionList : IList<string>
        {
            private readonly List<string> extensions;

            public ExtensionList()
            {
                extensions = new List<string>();
            }

            public ExtensionList(IEnumerable<string> collection)
            {
                extensions = new List<string>();
                foreach (string s in collection)
                    Add(s);
            }

            public ExtensionList(params string[] items)
            {
                extensions = new List<string>(items.Length);
                foreach (string s in items)
                    Add(s);
            }

            private string convert(string value)
            {
                if (value[0] != '.')
                    return "." + value;
                else
                    return value;
            }

            public override string ToString()
            {
                string s = "{";
                for (int i = 0; i < extensions.Count; i++)
                {
                    s += extensions[i];
                    if (i < extensions.Count - 1)
                        s += " | ";
                }
                s += "}";
                return s;
            }

            #region IList<string> Members

            public int IndexOf(string item)
            {
                return extensions.LastIndexOf(convert(item));
            }

            public void Insert(int index, string item)
            {
                extensions.Insert(index, convert(item));
            }

            public void RemoveAt(int index)
            {
                extensions.RemoveAt(index);
            }

            public string this[int index]
            {
                get { return extensions[index]; }
                set { extensions[index] = convert(value); }
            }

            #endregion

            #region ICollection<string> Members

            public void Add(string item)
            {
                extensions.Add(convert(item));
            }

            public void Clear()
            {
                extensions.Clear();
            }

            public bool Contains(string item)
            {
                return extensions.Contains(convert(item));
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                extensions.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return extensions.Count; }
            }

            public bool IsReadOnly
            {
                get { return (extensions as IList<string>).IsReadOnly; }
            }

            public bool Remove(string item)
            {
                return extensions.Remove(convert(item));
            }

            public string[] ToArray()
            {
                return extensions.ToArray();
            }

            #endregion

            #region IEnumerable<string> Members

            public IEnumerator<string> GetEnumerator()
            {
                return extensions.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (extensions as IEnumerable).GetEnumerator();
            }

            #endregion
        }
    }
}