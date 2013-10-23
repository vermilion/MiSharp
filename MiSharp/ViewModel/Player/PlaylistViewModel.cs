using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Model;
using Rareform.Extensions;
using Rareform.Reflection;

namespace MiSharp
{
    [Export(typeof (PlaylistViewModel))]
    public sealed class PlaylistViewModel : PropertyChangedBase, IDataErrorInfo, IDisposable, IHandle<Song>
    {
        private readonly IEventAggregator _events;
        private readonly Playlist _playlist = new Playlist("Default");
        private readonly Func<string, bool> _renameRequest;
        private List<PlaylistEntryViewModel> _currentEntries;
        private bool _editName;
        private string _saveName;
        private int? _songCount;

        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            _events = events;
            events.Subscribe(this);
        }

        public bool EditName
        {
            get { return _editName; }
            set
            {
                if (EditName != value)
                {
                    _editName = value;

                    if (EditName)
                    {
                        _saveName = Name;
                    }

                    else if (this.HasErrors())
                    {
                        Name = _saveName;
                        _saveName = null;
                    }

                    NotifyOfPropertyChange(() => EditName);
                }
            }
        }

        public string Name
        {
            get { return _playlist.Name; }
            set
            {
                if (Name != value)
                {
                    _playlist.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        public int SongCount
        {
            get
            {
                // We use this to get a value, even if the Songs property hasn't been called
                if (_songCount == null)
                {
                    return Songs.Count();
                }

                return _songCount.Value;
            }

            set
            {
                if (_songCount != value)
                {
                    _songCount = value;
                    NotifyOfPropertyChange(() => SongCount);
                }
            }
        }


        public IEnumerable<PlaylistEntryViewModel> Songs
        {
            get
            {
                DisposeCurrentEntries();
                List<PlaylistEntryViewModel> songs = _playlist
                    .Select(entry => new PlaylistEntryViewModel(entry))
                    .ToList(); // We want a list, so that ReSharper doesn't complain about multiple enumerations

                SongCount = songs.Count;

                if (_playlist.CurrentSongIndex.HasValue)
                {
                    PlaylistEntryViewModel entry = songs[_playlist.CurrentSongIndex.Value];

                    //if (!entry.IsCorrupted)
                    {
                        entry.IsPlaying = true;
                    }

                    // If there are more than 5 songs from the beginning of the playlist to the current played song,
                    // skip all, but 5 songs to the position of the currently played song
                    if (_playlist.CurrentSongIndex > 5)
                    {
                        songs = songs.Skip(_playlist.CurrentSongIndex.Value - 5).ToList();
                    }

                    foreach (PlaylistEntryViewModel model in songs.TakeWhile(song => !song.IsPlaying))
                    {
                        model.IsInactive = true;
                    }
                }

                _currentEntries = songs;

                return songs;
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == Reflector.GetMemberName(() => Name))
                {
                    if (!_renameRequest(Name))
                    {
                        error = "Name already exists.";
                    }

                    else if (String.IsNullOrWhiteSpace(Name))
                    {
                        error = "Name cannot be empty or whitespace.";
                    }
                }

                return error;
            }
        }

        public void Dispose()
        {
            DisposeCurrentEntries();
        }

        public void Handle(Song tag)
        {
            _playlist.AddSongs(new List<Song> {tag});
            NotifyOfPropertyChange(() => Songs);
        }

        public void PlayClick()
        {
            _events.Publish(_currentEntries);
        }

        private void DisposeCurrentEntries()
        {
            if (_currentEntries != null)
            {
                foreach (PlaylistEntryViewModel entry in _currentEntries)
                {
                    //  entry.Dispose();
                }
            }
        }
    }
}