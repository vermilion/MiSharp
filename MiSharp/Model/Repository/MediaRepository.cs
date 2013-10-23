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
            return GetAllSongs().Where(x => x.Artist == bandName)
                                .Distinct(new AlbumEqualityComparer())
                                .Select(x => new Album {Name = x.Album, Year = x.Year});
        }

        public IEnumerable<string> GetAllBands()
        {
            return GetAllSongs().Select(x => x.Artist).Distinct();
        }

        public IEnumerable<Song> GetAllSongsFiltered(TagFilter filter)
        {
            IEnumerable<Song> result = GetAllSongs();
            if (filter.BandName != null)
                result = result.Where(x => x.Artist == filter.BandName);
            if (filter.AlbumName != null)
                result = result.Where(x => x.Album == filter.AlbumName);
            return result;
        }

        public IEnumerable<Song> GetAllSongs()
        {
            return Repository.GetAll<Song>();
        }

        public event FileFoundEventHandler FileFound;
        public event ScanCompletedEventHandler ScanCompleted;

        //TODO:fk off events. Make Task<> based
        public async Task<bool> Rescan()
        {
            FileInfo[] files = Settings.Instance.FileFormats
                                       .SelectMany(f =>
                                                   new DirectoryInfo(Settings.Instance.WatchFolder)
                                                       .GetFiles(f.Trim(), SearchOption.AllDirectories))
                                       .ToArray();

            Int64 count = files.Count();
            for (int index = 0; index < files.Length; index++)
            {
                FileInfo file = files[index];
                FileFound(new FileStatEventargs {File = file, CurrentFileNumber = index, TotalFiles = count});
                try
                {
                    var tag = new Song(file.FullName);
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