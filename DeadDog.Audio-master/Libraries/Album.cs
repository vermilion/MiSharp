using System;
using DeadDog.Audio.Libraries.Collections;
using ReactiveUI;

namespace DeadDog.Audio.Libraries
{
    public class Album : ReactiveObject
    {
        #region Properties

        private Artist _artist;
        private bool _isunknown;

        private string _title;
        private uint _year;

        private TrackCollection _tracks;

        // This is correct! - Artist should NOT be a constructor argument.

        public bool IsUnknown
        {
            get { return _isunknown; }
            set { this.RaiseAndSetIfChanged(ref _isunknown, value); }
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        public TrackCollection Tracks
        {
            get { return _tracks; }
            set { this.RaiseAndSetIfChanged(ref _tracks, value); }
        }

        public Artist Artist
        {
            get { return _artist; }
            internal set { _artist = value; }
        }

        public uint Year
        {
            get { return _year; }
            set { this.RaiseAndSetIfChanged(ref _year, value); }
        }

        public bool HasArtist
        {
            get { return _artist != null; }
        }

        #endregion

        public Album(string album)
        {
            _isunknown = album == null;
            _tracks = new TrackCollection(TrackAdded, TrackRemoved);

            _title = album ?? string.Empty;
        }

        public override string ToString()
        {
            return _title;
        }

        private void TrackAdded(TrackCollection collection, TrackEventArgs e)
        {
            if (collection != _tracks)
                throw new InvalidOperationException("Album attempted to alter wrong trackcollection.");

            if (collection.Count == 1)
            {
                _artist = e.Track.Artist;
                _artist.Albums.Add(this);
            }
            else if (e.Track.Artist != null && e.Track.Artist != _artist)
            {
                if (_artist != null)
                    _artist.Albums.Remove(this);
                _artist = null;
            }
        }

        private void TrackRemoved(TrackCollection collection, TrackEventArgs e)
        {
            if (collection != _tracks)
                throw new InvalidOperationException("Album attempted to alter wrong trackcollection.");

            Artist temp = null;
            for (int i = 0; i < collection.Count; i++)
                if (temp == null)
                    temp = collection[i].Artist;
                else if (collection[i].Artist != null && collection[i].Artist != temp)
                {
                    if (_artist != null)
                        _artist.Albums.Remove(this);
                    _artist = null;
                    return;
                }

            // All track artist are the same (or null)
            if (_artist != null)
                _artist.Albums.Remove(this);

            _artist = temp;

            if (temp != null)
                _artist.Albums.Add(this);
        }
    }
}