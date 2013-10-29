using System.Collections.Generic;

namespace DeadDog.Audio
{
    internal class ReverseList<T>
    {
        private readonly List<PlaylistEntry<T>> list;
        private bool canremovetop;
        private int index;

        public ReverseList()
        {
            index = 0;
            list = new List<PlaylistEntry<T>>();
        }

        public PlaylistEntry<T> Current
        {
            get
            {
                if (list.Count == 0)
                    return null;
                else if (list.Count == index)
                    return null;
                else
                    return list[index];
            }
        }

        public bool CanRemoveTop
        {
            set { canremovetop = true; }
        }

        public bool MoveNext()
        {
            if (index == list.Count)
                return false;

            index++;
            if (index == list.Count)
                return false;

            return true;
        }

        public bool MovePrevious()
        {
            if (index == 0)
                return false;

            index--;
            return true;
        }

        public void Add(PlaylistEntry<T> entry)
        {
            if (entry == null)
                return;
            if (index == list.Count && canremovetop)
            {
                list.RemoveAt(list.Count - 1);
                index--;
                canremovetop = false;
            }

            if (index < list.Count)
                list.RemoveRange(index, list.Count - 1);

            list.Add(entry);
            index++;
        }

        public void Remove(PlaylistEntry<T> entry)
        {
            while (list.Contains(entry))
            {
                int i = list.IndexOf(entry);
                list.RemoveAt(i);
                if (i <= index)
                    index--;
            }
        }

        public void Clear()
        {
            index = 0;
            list.Clear();
        }
    }
}