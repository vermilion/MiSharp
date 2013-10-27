﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.Library;
using MiSharp.Core.Repository;

namespace MiSharp
{
    [Export(typeof (LibraryViewModel))]
    public class LibraryViewModel : PropertyChangedBase, IHandle<FileStatEventArgs>
    {
        public readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;
        private Album _selectedAlbum;
        private ArtistViewModel _selectedBand;

        [ImportingConstructor]
        public LibraryViewModel(IEventAggregator events, IWindowManager windowManager)
        {
            _events = events;
            _events.Subscribe(this);
            _windowManager = windowManager;
        }

        public string Status { get; set; }

        public ArtistViewModel SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                _selectedBand = value;
                NotifyOfPropertyChange(() => Albums);
            }
        }

        //TODO: multiple selection
        public Song SelectedSong { get; set; }


        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                NotifyOfPropertyChange(() => Songs);
            }
        }

        public IEnumerable<ArtistViewModel> Bands
        {
            get { return MediaRepository.Instance.GetAllBands().OrderBy(x => x).Select(x => new ArtistViewModel(x)); }
        }

        public IEnumerable<Album> Albums
        {
            get
            {
                if (SelectedBand == null) return new List<Album>();
                return MediaRepository.Instance.GetAllAlbums(SelectedBand.Name);
            }
        }

        public IEnumerable<Song> SongsAll
        {
            get
            {
                IEnumerable<Song> list = MediaRepository.Instance.GetAllSongsFiltered(new TagFilter("Hurt", "The Crux"));
                return list;
            }
        }

        public IEnumerable<Song> Songs
        {
            get
            {
                if (SelectedBand == null || SelectedAlbum == null) return new List<Song>();
                return
                    MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(SelectedBand.Name, SelectedAlbum.Name));
            }
        }

        public void AddArtistToPlaylist()
        {
            _events.Publish(GetSongsByArtist());
        }

        public void AddAlbumToPlaylist()
        {
            _events.Publish(GetSongsByAlbum());
        }

        public void AddAlbumToPlaylistNew(string albumName)
        {
            _events.Publish(GetSongsByAlbumNew(albumName));
        }

        public List<Song> GetSongsByAlbumNew(string albumName)
        {
            return MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(null, albumName)).ToList();
        }

        public void AddSongToPlaylist()
        {
            _events.Publish(new List<Song> {SelectedSong});
        }

        private List<Song> GetSongsByArtist()
        {
            return MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(SelectedBand.Name, null)).ToList();
        }

        private List<Song> GetSongsByAlbum()
        {
            return MediaRepository.Instance.GetAllSongsFiltered(new TagFilter(null, SelectedAlbum.Name)).ToList();
        }


        #region TagEditor

        public void EditorEditAlbums()
        {
            if (SelectedAlbum != null)
                _windowManager.ShowDialog(new AlbumTagEditorViewModel(GetSongsByAlbum()));
        }

        public void EditorEditAlbumsNew(string albumName)
        {
            _windowManager.ShowDialog(new AlbumTagEditorViewModel(GetSongsByAlbumNew(albumName)));
        }

        public void EditorEditArtists()
        {
            if (SelectedBand != null)
                _windowManager.ShowDialog(new ArtistTagEditorViewModel(GetSongsByArtist()));
        }

        public void EditorEditSongs()
        {
            if (SelectedSong != null)
                _windowManager.ShowDialog(new SongTagEditorViewModel(new List<Song> {SelectedSong}));
        }

        #endregion

        #region IHandle

        public void Handle(FileStatEventArgs e)
        {
            Status = e.CurrentFileNumber + ":" + e.TotalFiles;
            NotifyOfPropertyChange(() => Status);
            if (e.Completed)
                NotifyOfPropertyChange(() => Bands);
        }

        #endregion
    }
}