using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiSharp.Model.EqualityComparers;
using MiSharp.Model.Library;

namespace MiSharp.Model.Repository
{
    public class MediaRepository : RepositoryBase
    {
        public delegate void FileFoundEventHandler(FileStatEventargs e);

        public delegate void ScanCompletedEventHandler();


        private const string LibPath = "media.lib";
        private static MediaRepository _library;

        public MediaRepository() : base(LibPath)
        {
        }

        public static MediaRepository Instance
        {
            get { return _library ?? (_library = new MediaRepository()); }
        }

        public IEnumerable<Album> GetAllAlbums(string bandName)
        {
            return GetAllSongs().Where(x => x.AlbumArtist == bandName)
                .Distinct(new AlbumEqualityComparer())
                .Select(x => new Album {Name = x.Album, Year = x.Year});
        }

        public IEnumerable<string> GetAllBands()
        {
            return GetAllSongs().Select(x => x.AlbumArtist).Distinct();
        }

        public IEnumerable<Tag> GetAllSongsFiltered(TagFilter filter)
        {
            IEnumerable<Tag> result = GetAllSongs();
            if (filter.BandName != null)
                result = result.Where(x => x.AlbumArtist == filter.BandName);
            if (filter.AlbumName != null)
                result = result.Where(x => x.Album == filter.AlbumName);
            return result;
        }

        public IEnumerable<Tag> GetAllSongs()
        {
            return Repository.GetAll<Tag>();
        }

        public event FileFoundEventHandler FileFound;
        public event ScanCompletedEventHandler ScanCompleted;

        //TODO:fk off events. Make Task<> based
        public async Task<bool> Rescan()
        {
            FileInfo[] files = new DirectoryInfo(Settings.Instance.WatchFolder).GetFiles("*.mp3",
                SearchOption.AllDirectories);
            Int64 count = files.Count();
            for (int index = 0; index < files.Length; index++)
            {
                FileInfo file = files[index];
                FileFound(new FileStatEventargs {File = file, CurrentFileNumber = index, TotalFiles = count});
                try
                {
                    var tag = new Tag(file.FullName);
                    await Repository.Save(tag);
                }
                catch
                {
//TODO: dunno why crashes on Tag gen. investigate it
                }
            }
            ScanCompleted();
            return true;
        }
    }
}