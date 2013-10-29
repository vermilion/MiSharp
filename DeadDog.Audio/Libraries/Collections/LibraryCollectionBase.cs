using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeadDog.Audio.Libraries.Collections
{
    public abstract class LibraryCollectionBase<T> : IEnumerable<T> where T : class
    {
        private readonly List<T> _list;

        internal LibraryCollectionBase()
        {
            _list = new List<T>();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }

        public T this[string name]
        {
            get { return _list.FirstOrDefault(e => GetName(e) == name); }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>) _list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        protected abstract string GetName(T element);
        protected abstract int Compare(T element1, T element2);

        public bool Contains(T element)
        {
            return _list.BinarySearch(element, Compare) >= 0;
        }

        public bool Contains(string name)
        {
            return _list.BinarySearch(name, string.Compare, GetName) >= 0;
        }

        internal void Add(T element)
        {
            int index = _list.BinarySearch(element, Compare);
            if (index < 0) index = ~index;
            _list.Insert(index, element);

            OnAdded(element);
        }

        internal void Remove(T element)
        {
            _list.Remove(element);

            OnRemoved(element);
        }

        internal void Reposition(T element)
        {
            _list.Remove(element);

            int index = _list.BinarySearch(element, Compare);
            if (index < 0) index = ~index;
            _list.Insert(index, element);
        }

        protected abstract void OnAdded(T element);
        protected abstract void OnRemoved(T element);

        public override string ToString()
        {
            return "Count {" + _list.Count + "}";
        }
    }
}