using System.IO;
using ReactiveUI;

namespace DeadDog.Audio.Libraries
{
    public class Track : ReactiveObject
    {
        #region Fields and properties

        private readonly FileInfo _file;
        private string _title;
        private int? _tracknumber;

        public bool FileExist
        {
            get
            {
                _file.Refresh();
                return _file.Exists;
            }
        }

        public string FilePath
        {
            get { return _file.FullName; }
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        public int? Tracknumber
        {
            get { return _tracknumber; }
            set { this.RaiseAndSetIfChanged(ref _tracknumber, value); }
        }

        public Album Album { get; internal set; }

        public Artist Artist { get; internal set; }

        public RawTrack Model { get; set; }

        #endregion

        public Track(RawTrack trackinfo, Album album, Artist artist)
        {
            _file = trackinfo.File;
            Album = album;
            Artist = artist;
            _title = trackinfo.TrackTitle;
            _tracknumber = trackinfo.TrackNumberUnknown ? (int?) null : trackinfo.TrackNumber;
            Model = trackinfo;
        }

        public override string ToString()
        {
            return (_tracknumber == null ? "" : "#" + _tracknumber + " ") + _title;
        }
    }
}