using MiSharp.Model;

namespace MiSharp
{
    public sealed class PlaylistEntryViewModel : Song
    {
        private readonly PlaylistEntry _entry;
        private bool _isInactive;
        private bool _isPlaying;

        public PlaylistEntryViewModel(PlaylistEntry entry)
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
                    //NotifyOfPropertyChange(() => IsInactive);
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
                    //NotifyOfPropertyChange(() => IsPlaying);
                }
            }
        }
    }
}