﻿using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Libraries.Collections;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    public class ArtistViewModel : CoverViewModelBase
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _windowManager;

        public ArtistViewModel(Artist artist)
        {
            Model = artist;
            Albums = artist.Albums;
            _events = IoC.Get<IEventAggregator>();
            _windowManager = IoC.Get<IWindowManager>();

            AddArtistToPlaylistCommand = new ReactiveCommand();
            AddArtistToPlaylistCommand.Subscribe(
                param => _events.Publish(Albums.SelectMany(x => x.Tracks).Select(x => x.Model).ToList()));

            EditorEditArtistsCommand = new ReactiveCommand();
            EditorEditArtistsCommand.Subscribe(param => _windowManager.ShowDialog(
                new ArtistTagEditorViewModel(Albums.SelectMany(x => x.Tracks).Select(x => x.Model).ToList())));

            Func = () => CoverRepository.Instance.GetArtistCover(Model.Name);
        }

        public ReactiveCommand AddArtistToPlaylistCommand { get; private set; }
        public ReactiveCommand EditorEditArtistsCommand { get; private set; }

        #region Properties

        public Artist Model { get; set; }
        public AlbumCollection Albums { get; set; }

        public int SongsCount
        {
            get { return Albums.Sum(x => x.Tracks.Count); }
        }

        public int AlbumsCount
        {
            get { return Albums.Count; }
        }

        #endregion
    }
}