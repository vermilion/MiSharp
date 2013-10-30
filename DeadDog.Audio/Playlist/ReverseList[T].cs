using System.Collections.Generic;

namespace DeadDog.Audio.Playlist
{
    internal class ReverseList<T>
    {
        private readonly List<T> _list;
        private bool _canremovetop;
        private int _index;

        public ReverseList()
        {
            _index = 0;
            _list = new List<T>();
        }

        public T Current
        {
            get
            {
                if (_list.Count == 0)
                    return default(T);
                if (_list.Count == _index)
                    return default(T);
                return _list[_index];
            }
        }

        public bool CanRemoveTop
        {
            set { _canremovetop = true; }
        }

        public bool MoveNext()
        {
            if (_index == _list.Count)
                return false;

            _index++;
            if (_index == _list.Count)
                return false;

            return true;
        }

        public bool MovePrevious()
        {
            if (_index == 0)
                return false;

            _index--;
            return true;
        }

        public void Add(T entry)
        {
            if (entry == null)
                return;
            if (_index == _list.Count && _canremovetop)
            {
                _list.RemoveAt(_list.Count - 1);
                _index--;
                _canremovetop = false;
            }

            if (_index < _list.Count)
                _list.RemoveRange(_index, _list.Count - 1);

            _list.Add(entry);
            _index++;
        }

        public void Remove(T entry)
        {
            while (_list.Contains(entry))
            {
                int i = _list.IndexOf(entry);
                _list.RemoveAt(i);
                if (i <= _index)
                    _index--;
            }
        }

        public void Clear()
        {
            _index = 0;
            _list.Clear();
        }
    }
}