using System;

namespace DeadDog.Audio.Libraries.Collections
{
    public class TrackCollection : LibraryCollectionBase<Track>
    {
        private readonly TrackEventHandler _addHandler;
        private readonly TrackEventHandler _removeHandler;

        internal TrackCollection()
            : this(null, null)
        {
        }

        internal TrackCollection(TrackEventHandler addHandler, TrackEventHandler removeHandler)
        {
            if ((addHandler == null || removeHandler == null) && addHandler != removeHandler)
                throw new ArgumentException("Both addHandler and removeHandler must be specified.");

            _addHandler = addHandler;
            _removeHandler = removeHandler;
        }

        public event TrackEventHandler TrackAdded , TrackRemoved;

        protected override void OnAdded(Track element)
        {
            var e = new TrackEventArgs(element);
            if (_addHandler != null)
                _addHandler(this, e);
            if (TrackAdded != null)
                TrackAdded(this, e);
        }

        protected override void OnRemoved(Track element)
        {
            var e = new TrackEventArgs(element);
            if (_removeHandler != null)
                _removeHandler(this, e);
            if (TrackRemoved != null)
                TrackRemoved(this, e);
        }

        protected override string GetName(Track element)
        {
            return element.Title;
        }

        protected override int Compare(Track element1, Track element2)
        {
            int? v1 = element1.Tracknumber, v2 = element2.Tracknumber;
            if (v1.HasValue)
                return v2.HasValue ? v1.Value.CompareTo(v2.Value) : 1;
            else
                return v2.HasValue ? -1 : 0;
        }
    }
}