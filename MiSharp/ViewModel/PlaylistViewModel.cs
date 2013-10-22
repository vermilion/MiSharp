using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Model;

namespace MiSharp
{
    public class Playlist
    {
        public ObservableCollection<Tag> TagPlaylist { get; set; }
        public int CurrentIndex { get; set; }
    }

    [Export(typeof (PlaylistViewModel))]
    public class PlaylistViewModel : PropertyChangedBase, IHandle<Tag>
    {
        private readonly IEventAggregator _events;
        private Tag _currentTag;
        private ObservableCollection<Tag> _playlist = new ObservableCollection<Tag>();

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            _events = events;
            events.Subscribe(this);
        }

        public ObservableCollection<Tag> Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;
                NotifyOfPropertyChange(() => Playlist);
            }
        }

        public Tag CurrentTag
        {
            get { return _currentTag; }
            set
            {
                _currentTag = value;
                NotifyOfPropertyChange(() => CurrentTag);
            }
        }

        public void PlayClick()
        {
            _events.Publish(new Playlist
                {
                    TagPlaylist = Playlist,
                    CurrentIndex = Playlist.IndexOf(CurrentTag)
                });
        }

        public void RemoveSelected()
        {
            Playlist.Remove(CurrentTag);
        }

        #region IHandle implementation

        public void Handle(Tag tag)
        {
            Playlist.Add(tag);
        }

        #endregion
    }
}