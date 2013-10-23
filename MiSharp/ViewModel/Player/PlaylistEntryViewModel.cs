using System;

namespace MiSharp
{
    public sealed class PlaylistEntryViewModel : SongViewModelBase
    {
        private readonly PlaylistEntry _entry;
        private bool _isInactive;
        private bool _isPlaying;

        public PlaylistEntryViewModel(PlaylistEntry entry)
            : base(entry.Song)
        {
            _entry = entry;
        }

        public int Index
        {
            get { return _entry.Index; }
        }

        public bool IsInactive
        {
            get { return _isInactive; }
            set
            {
                if (IsInactive != value)
                {
                    _isInactive = value;
                    NotifyOfPropertyChange(() => IsInactive);
                }
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (IsPlaying != value)
                {
                    _isPlaying = value;
                    NotifyOfPropertyChange(() => IsPlaying);
                }
            }
        }

        public string Source
        {
            get
            {
                if (Model != null)
                {
                    return "Local";
                }

                throw new InvalidOperationException();
            }
        }
    }
}