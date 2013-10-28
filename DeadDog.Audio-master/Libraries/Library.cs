using System;
using System.Collections.Generic;
using DeadDog.Audio.Libraries.Collections;

namespace DeadDog.Audio.Libraries
{
    public class Library
    {
        private readonly AlbumCollection _albums;
        private readonly ArtistCollection _artists;
        private readonly Dictionary<string, Track> _trackDict;
        private readonly TrackCollection _tracks;

        public Library()
        {
            _artists = new ArtistCollection();
            _albums = new AlbumCollection();
            _tracks = new TrackCollection();

            _trackDict = new Dictionary<string, Track>();
        }

        public ArtistCollection Artists
        {
            get { return _artists; }
        }

        public AlbumCollection Albums
        {
            get { return _albums; }
        }

        public TrackCollection Tracks
        {
            get { return _tracks; }
        }

        public Track AddTrack(RawTrack track)
        {
            if (track == null)
                throw new ArgumentNullException("track");

            if (_trackDict.ContainsKey(track.FullFilename))
                throw new ArgumentException(track.FullFilename + " is already in library - use Update instead.", "track");

            Artist artist = _artists[track.ArtistName] ?? CreateArtist(track.ArtistName);
            Album album = _albums[track.AlbumTitle] ?? CreateAlbum(track.AlbumTitle);

            var t = new Track(track, album, artist);
            _tracks.Add(t);
            album.Tracks.Add(t);

            _trackDict.Add(track.FullFilename, t);

            return t;
        }

        private Artist CreateArtist(string artistname)
        {
            var artist = new Artist(artistname);
            _artists.Add(artist);
            return artist;
        }

        private Album CreateAlbum(string albumName)
        {
            var album = new Album(albumName);
            _albums.Add(album);
            return album;
        }

        public Track UpdateTrack(RawTrack track)
        {
            Track old;
            if (!_trackDict.TryGetValue(track.FullFilename, out old))
                throw new ArgumentException(track.FullFilename + " was not found in library - use Add instead.", "track");

            old.Title = track.TrackTitle;
            old.Tracknumber = track.TrackNumberUnknown ? (int?) null : track.TrackNumber;

            Album album = _albums[track.AlbumTitle] ?? CreateAlbum(track.AlbumTitle);
            if (album != old.Album)
            {
                old.Album.Tracks.Remove(old);
                if (old.Album.Tracks.Count == 0 && !old.Album.IsUnknown)
                    _albums.Remove(old.Album);

                old.Album = album;
                album.Tracks.Add(old);
            }
            else
                album.Tracks.Reposition(old);

            Artist artist = _artists[track.ArtistName] ?? CreateArtist(track.ArtistName);
            if (artist != old.Artist)
            {
                Artist oldArtist = old.Artist;
                old.Artist = artist;

                old.Album.Tracks.Remove(old);
                old.Album.Tracks.Add(old);
                if (oldArtist.Albums.Count == 0 && oldArtist.Albums.UnknownAlbum.Tracks.Count == 0)
                    _artists.Remove(oldArtist);
            }

            return old;
        }

        public void RemoveTrack(Track track)
        {
            Album album = track.Album;
            Artist artist = track.Artist;

            _tracks.Remove(track);
            if (album != null)
            {
                album.Tracks.Remove(track);
                if (album.Tracks.Count == 0 && !album.IsUnknown)
                    _albums.Remove(album);
            }

            if (artist != null)
            {
                if (artist.Albums.Count == 0 && artist.Albums.UnknownAlbum.Tracks.Count == 0)
                    _artists.Remove(artist);
            }

            _trackDict.Remove(track.FilePath);
        }

        public void RemoveTrack(string filename)
        {
            RemoveTrack(_trackDict[filename]);
        }
    }
}