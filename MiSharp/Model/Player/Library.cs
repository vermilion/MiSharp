using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MiSharp.Model;
using Rareform.Extensions;
using Rareform.Validation;
using TagLib;

namespace MiSharp
{
    public sealed class Library : IDisposable
    {
        private readonly AutoResetEvent cacheResetHandle;

        // We need a lock when disposing songs to prevent a modification of the enumeration
        private readonly object disposeLock;

        private readonly List<Playlist> playlists;
        private readonly object songLock;
        private readonly HashSet<Song> songs;
        private bool abortSongAdding;
        private AudioPlayer currentPlayer;
        private Playlist currentPlayingPlaylist;
        private bool isWaitingOnCache;
        private DateTime lastSongAddTime;
        public Library()
        {
            songLock = new object();
            songs = new HashSet<Song>();
            playlists = new List<Playlist>();
            cacheResetHandle = new AutoResetEvent(false);
            disposeLock = new object();
        }

        /// <summary>
        ///     Gets a value indicating whether the next song in the playlist can be played.
        /// </summary>
        /// <value>
        ///     true if the next song in the playlist can be played; otherwise, false.
        /// </value>
        public bool CanPlayNextSong
        {
            get { return CurrentPlaylist.CanPlayNextSong; }
        }

        /// <summary>
        ///     Gets a value indicating whether the previous song in the playlist can be played.
        /// </summary>
        /// <value>
        ///     true if the previous song in the playlist can be played; otherwise, false.
        /// </value>
        public bool CanPlayPreviousSong
        {
            get { return CurrentPlaylist.CanPlayPreviousSong; }
        }

        public Playlist CurrentPlaylist { get; private set; }

        /// <summary>
        ///     Gets or sets the current song's elapsed time.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentPlayer == null ? TimeSpan.Zero : currentPlayer.CurrentTime; }
            set { currentPlayer.CurrentTime = value; }
        }

        //public bool EnablePlaylistTimeout
        //{
        //    get { return this.settings.EnablePlaylistTimeout; }
        //    set
        //    {
        //        this.settings.EnablePlaylistTimeout = value;
        //    }
        //}

        /// <summary>
        ///     Gets a value indicating whether the playback is paused.
        /// </summary>
        /// <value>
        ///     true if the playback is paused; otherwise, false.
        /// </value>
        public bool IsPaused
        {
            get { return currentPlayer != null && currentPlayer.PlaybackState == AudioPlayerState.Paused; }
        }

        /// <summary>
        ///     Gets a value indicating whether the playback is started.
        /// </summary>
        /// <value>
        ///     true if the playback is started; otherwise, false.
        /// </value>
        public bool IsPlaying
        {
            get { return currentPlayer != null && currentPlayer.PlaybackState == AudioPlayerState.Playing; }
        }

        /// <summary>
        ///     Gets the song that is currently loaded.
        /// </summary>
        public Song LoadedSong
        {
            get { return currentPlayer == null ? null : currentPlayer.Song; }
        }

        public IEnumerable<Playlist> Playlists
        {
            get { return playlists; }
        }

        //public TimeSpan PlaylistTimeout
        //{
        //    get { return this.settings.PlaylistTimeout; }
        //    set
        //    {
        //        this.settings.PlaylistTimeout = value;
        //    }
        //}

        //public TimeSpan RemainingPlaylistTimeout
        //{
        //    get
        //    {
        //        return this.lastSongAddTime + this.PlaylistTimeout <= DateTime.Now
        //                   ? TimeSpan.Zero
        //                   : this.lastSongAddTime - DateTime.Now + this.PlaylistTimeout;
        //    }
        //}

        /// <summary>
        ///     Gets all songs that are currently in the library.
        /// </summary>
        public IEnumerable<Song> Songs
        {
            get
            {
                IEnumerable<Song> tempSongs;

                lock (songLock)
                {
                    tempSongs = songs.ToList();
                }

                return tempSongs;
            }
        }

        /// <summary>
        ///     Gets the duration of the current song.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return currentPlayer == null ? TimeSpan.Zero : currentPlayer.TotalTime; }
        }

        public void Dispose()
        {
            if (currentPlayer != null)
            {
                currentPlayer.Dispose();
            }

            abortSongAdding = true;

            cacheResetHandle.Dispose();

            lock (disposeLock)
            {
                //DisposeSongs(this.songs);
            }

            //settings.Save();
        }

        /// <summary>
        ///     Occurs when the playlist has changed.
        /// </summary>
        public event EventHandler PlaylistChanged;

        /// <summary>
        ///     Occurs when a corrupted song has been attempted to be played.
        /// </summary>
        public event EventHandler SongCorrupted;

        /// <summary>
        ///     Occurs when a song has finished the playback.
        /// </summary>
        public event EventHandler SongFinished;

        /// <summary>
        ///     Occurs when a song has started the playback.
        /// </summary>
        public event EventHandler SongStarted;

        /// <summary>
        ///     Occurs when the library is finished with updating.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        ///     Occurs when the library is updating.
        /// </summary>
        public event EventHandler Updating;

        //public float Volume
        //{
        //    get { return settings.Volume; }
        //    set
        //    {
        //        settings.Volume = value;
        //        if (this.currentPlayer != null)
        //        {
        //            this.currentPlayer.Volume = value;
        //        }
        //    }
        //}
        /// <summary>
        ///     Gets or sets the current volume.
        /// </summary>
        /// <value>
        ///     The current volume.
        /// </value>
        /// <summary>
        ///     Adds a new playlist to the library and immediately sets it as the current playlist.
        /// </summary>
        /// <param name="name">The name of the playlist, It is required that no other playlist has this name.</param>
        /// <exception cref="InvalidOperationException">A playlist with the specified name already exists.</exception>
        public void AddAndSwitchToPlaylist(string name)
        {
            AddPlaylist(name);
            SwitchToPlaylist(GetPlaylistByName(name));
        }

        //public Task AddLocalSongsAsync(string path)
        //{
        //    if (path == null)
        //        Throw.ArgumentNullException(() => path);
        //    return Task.Factory.StartNew(() => this.AddLocalSongs(path));
        //}
        /// <summary>
        ///     Adds the song that are contained in the specified directory recursively in an asynchronous manner to the library.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>
        ///     The <see cref="Task" /> that did the work.
        /// </returns>
        /// <summary>
        ///     Adds a new playlist with the specified name to the library.
        /// </summary>
        /// <param name="name">The name of the playlist. It is required that no other playlist has this name.</param>
        /// <exception cref="InvalidOperationException">A playlist with the specified name already exists.</exception>
        public void AddPlaylist(string name)
        {
            if (name == null)
                Throw.ArgumentNullException(() => name);

            if (GetPlaylistByName(name) != null)
                throw new InvalidOperationException("A playlist with this name already exists.");

            playlists.Add(new Playlist(name));
        }

        /// <summary>
        ///     Adds the specified song to the end of the playlist.
        ///     This method is only available in administrator mode.
        /// </summary>
        /// <param name="songList">The songs to add to the end of the playlist.</param>
        public void AddSongsToPlaylist(IEnumerable<Song> songList)
        {
            if (songList == null)
                Throw.ArgumentNullException(() => songList);


            CurrentPlaylist.AddSongs(songList.ToList()); // Copy the sequence to a list, so that the enumeration doesn't gets modified

            PlaylistChanged.RaiseSafe(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds the song to the end of the playlist.
        ///     This method throws an exception, if there is an outstanding timeout.
        /// </summary>
        /// <param name="song">The song to add to the end of the playlist.</param>
        public void AddSongToPlaylist(Song song)
        {
            if (song == null)
                Throw.ArgumentNullException(() => song);

            if (CurrentPlaylist == null) CurrentPlaylist = new Playlist("Default");
            CurrentPlaylist.AddSongs(new[] {song});

            lastSongAddTime = DateTime.Now;

            PlaylistChanged.RaiseSafe(this, EventArgs.Empty);
        }


        /// <summary>
        ///     Continues the currently loaded song.
        /// </summary>
        public void ContinueSong()
        {
            currentPlayer.Play();
        }


        public Playlist GetPlaylistByName(string playlistName)
        {
            if (playlistName == null)
                Throw.ArgumentNullException(() => playlistName);

            return playlists.FirstOrDefault(playlist => playlist.Name == playlistName);
        }

        public void Initialize()
        {
            Load();
        }

        /// <summary>
        ///     Pauses the currently loaded song.
        /// </summary>
        public void PauseSong()
        {
            currentPlayer.Pause();
        }

        /// <summary>
        ///     Plays the next song in the playlist.
        /// </summary>
        public void PlayNextSong()
        {
            InternPlayNextSong();
        }

        /// <summary>
        ///     Plays the previous song in the playlist.
        /// </summary>
        public void PlayPreviousSong()
        {
            if (!CurrentPlaylist.CanPlayPreviousSong || !CurrentPlaylist.CurrentSongIndex.HasValue)
                throw new InvalidOperationException("The previous song couldn't be played.");

            PlaySong(CurrentPlaylist.CurrentSongIndex.Value - 1);
        }

        /// <summary>
        ///     Plays the song with the specified index in the playlist.
        /// </summary>
        /// <param name="playlistIndex">The index of the song in the playlist.</param>
        public void PlaySong(int playlistIndex)
        {
            if (playlistIndex < 0)
                Throw.ArgumentOutOfRangeException(() => playlistIndex, 0);

            InternPlaySong(playlistIndex);
        }

        /// <summary>
        ///     Removes the specified songs from the library.
        /// </summary>
        /// <param name="songList">The list of the songs to remove from the library.</param>
        public void RemoveFromLibrary(IEnumerable<Song> songList)
        {
            if (songList == null)
                Throw.ArgumentNullException(() => songList);

            // DisposeSongs(songList);

            lock (songLock)
            {
                playlists.ForEach(playlist => RemoveFromPlaylist(playlist, songList));

                foreach (Song song in songList)
                {
                    songs.Remove(song);
                }
            }
        }

        /// <summary>
        ///     Removes the songs with the specified indexes from the playlist.
        /// </summary>
        /// <param name="indexes">The indexes of the songs to remove from the playlist.</param>
        public void RemoveFromPlaylist(IEnumerable<int> indexes)
        {
            if (indexes == null)
                Throw.ArgumentNullException(() => indexes);

            RemoveFromPlaylist(CurrentPlaylist, indexes);
        }

        /// <summary>
        ///     Removes the specified songs from the playlist.
        /// </summary>
        /// <param name="songList">The songs to remove.</param>
        public void RemoveFromPlaylist(IEnumerable<Song> songList)
        {
            if (songList == null)
                Throw.ArgumentNullException(() => songList);

            RemoveFromPlaylist(CurrentPlaylist, songList);
        }

        /// <summary>
        ///     Removes the playlist with the specified name from the library.
        /// </summary>
        /// <param name="playlistName">The name of the playlist to remove.</param>
        /// <exception cref="InvalidOperationException">No playlist exists, or no playlist with the specified name exists.</exception>
        public void RemovePlaylist(string playlistName)
        {
            if (playlistName == null)
                Throw.ArgumentNullException(() => playlistName);

            if (!Playlists.Any())
                throw new InvalidOperationException("There are no playlists.");

            Playlist playlist = GetPlaylistByName(playlistName);

            if (playlist == null)
                throw new InvalidOperationException("No playlist with the specified name exists.");

            playlists.Remove(playlist);
        }

        public void Save()
        {
            //HashSet<Song> casted;

            //lock (this.songLock)
            //{
            //    casted = new HashSet<Song>(this.songs.Cast<Song>());
            //}

            //// Clean up the songs that aren't in the library anymore
            //// This happens when the user adds a song from a removable device to the playlistx
            //// then removes the device, so the song is removed from the library, but not from the playlist
            //foreach (Playlist playlist in this.playlists)
            //{
            //    List<Song> removable = playlist.OfType<Song>().Where(song => !casted.Contains(song)).Cast<Song>().ToList();

            //    IEnumerable<int> indexes = playlist.GetIndexes(removable);

            //    playlist.RemoveSongs(indexes);
            //}

            //this.libraryWriter.Write(casted, this.playlists);
        }

        public void ShufflePlaylist()
        {
            CurrentPlaylist.Shuffle();
        }

        public void SwitchToPlaylist(Playlist playlist)
        {
            if (playlist == null)
                Throw.ArgumentNullException(() => playlist);

            CurrentPlaylist = playlist;
        }

        private void HandleSongFinish()
        {
            if (!CurrentPlaylist.CanPlayNextSong)
            {
                CurrentPlaylist.CurrentSongIndex = null;
            }

            currentPlayer.Dispose();
            currentPlayer = null;

            SongFinished.RaiseSafe(this, EventArgs.Empty);

            if (CurrentPlaylist.CanPlayNextSong)
            {
                InternPlayNextSong();
            }
        }

        private void InternPlayNextSong()
        {
            if (!CurrentPlaylist.CanPlayNextSong || !CurrentPlaylist.CurrentSongIndex.HasValue)
                throw new InvalidOperationException("The next song couldn't be played.");

            int nextIndex = CurrentPlaylist.CurrentSongIndex.Value + 1;
            Song nextSong = CurrentPlaylist[nextIndex].Song;

            // We want the to swap the songs, if the song that should be played next is currently caching
            if (CurrentPlaylist.ContainsIndex(nextIndex + 1))
            {
                PlaylistEntry nextReady = CurrentPlaylist
                    .Skip(nextIndex)
                    .FirstOrDefault();

                if (nextReady != null)
                {
                    CurrentPlaylist.InsertMove(nextReady.Index, nextIndex);
                }
            }

            InternPlaySong(nextIndex);
        }

        private void HandleSongCorruption()
        {
            if (!CurrentPlaylist.CanPlayNextSong)
            {
                CurrentPlaylist.CurrentSongIndex = null;
            }

            else
            {
                InternPlayNextSong();
            }
        }

        private void InternPlaySong(int playlistIndex)
        {
            if (playlistIndex < 0)
                Throw.ArgumentOutOfRangeException(() => playlistIndex, 0);

            if (isWaitingOnCache)
            {
                overrideCurrentCaching = true;

                // Let the song that is selected to be played wait here, if there is currently another song caching
                cacheResetHandle.WaitOne();
            }

            if (currentPlayingPlaylist != null)
            {
                currentPlayingPlaylist.CurrentSongIndex = null;
            }

            currentPlayingPlaylist = CurrentPlaylist;

            CurrentPlaylist.CurrentSongIndex = playlistIndex;

            Song song = CurrentPlaylist[playlistIndex].Song;

            RenewCurrentPlayer(song);

            Task.Factory.StartNew(() =>
                {
                    overrideCurrentCaching = false;

                    try
                    {
                        currentPlayer.Load();
                    }

                    catch (SongLoadException)
                    {
                        //song.IsCorrupted = true;
                        SongCorrupted.RaiseSafe(this, EventArgs.Empty);

                        HandleSongCorruption();

                        return;
                    }

                    try
                    {
                        currentPlayer.Play();
                    }

                    catch (PlaybackException)
                    {
                        //song.IsCorrupted = true;
                        SongCorrupted.RaiseSafe(this, EventArgs.Empty);

                        HandleSongCorruption();

                        return;
                    }

                    SongStarted.RaiseSafe(this, EventArgs.Empty);
                });
        }

        private void Load()
        {
            //IEnumerable<Song> savedSongs = this.libraryReader.ReadSongs();

            //foreach (Song song in savedSongs)
            //{
            //    this.songs.Add(song);
            //}

            //IEnumerable<Playlist> savedPlaylists = this.libraryReader.ReadPlaylists();

            //this.playlists.AddRange(savedPlaylists);
        }

        private void RemoveFromPlaylist(Playlist playlist, IEnumerable<int> indexes)
        {
            bool stopCurrentSong = playlist == CurrentPlaylist && indexes.Any(index => index == CurrentPlaylist.CurrentSongIndex);

            playlist.RemoveSongs(indexes);

            PlaylistChanged.RaiseSafe(this, EventArgs.Empty);

            if (stopCurrentSong)
            {
                currentPlayer.Stop();
                SongFinished.RaiseSafe(this, EventArgs.Empty);
            }
        }

        private void RemoveFromPlaylist(Playlist playlist, IEnumerable<Song> songList)
        {
            RemoveFromPlaylist(playlist, playlist.GetIndexes(songList));
        }

        private void RenewCurrentPlayer(Song song)
        {
            if (currentPlayer != null)
            {
                currentPlayer.Dispose();
            }

            currentPlayer = song.CreateAudioPlayer();

            currentPlayer.SongFinished += (sender, e) => HandleSongFinish();
            // this.currentPlayer.Volume = this.Volume;
        }
    }
}