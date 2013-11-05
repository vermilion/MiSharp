using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MiSharp.Core.Repository.FileSystem
{
    public class AlbumFileSystemCoverRepository : FileSystemCoverRepository
    {
        private static AlbumFileSystemCoverRepository _instance;

        public AlbumFileSystemCoverRepository()
        {
            CoversPath = @"Covers\Albums";
            InitDictionary();
        }

        public static AlbumFileSystemCoverRepository Instance
        {
            get { return _instance ?? (_instance = new AlbumFileSystemCoverRepository()); }
        }

        public async Task<BitmapImage> GetCover(string key, string artist)
        {
            if (!NameCoverDictionary.ContainsKey(key))
            {
                //try download
                string url = Api.LoadAlbumCover(key, artist);
                if (url != null)
                    return await LoadFromWeb(url, key);
            }
            else
                return await LoadFromDisk(key);

            return null;
        }
    }
}