using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MiSharp.Core.Repository.FileSystem
{
    public class ArtistFileSystemCoverRepository : FileSystemCoverRepository
    {
        private static ArtistFileSystemCoverRepository _instance;

        public ArtistFileSystemCoverRepository()
        {
            CoversPath = @"Covers\Artists";
            InitDictionary();
        }

        public static ArtistFileSystemCoverRepository Instance
        {
            get { return _instance ?? (_instance = new ArtistFileSystemCoverRepository()); }
        }

        public async Task<BitmapImage> GetCover(string key)
        {
            if (!NameCoverDictionary.ContainsKey(key))
            {
                //try download
                string url = Api.LoadArtistCover(key);
                if (url != null)
                    return await LoadFromWeb(url, key);
            }
            else
                return await LoadFromDisk(key);

            return null;
        }
    }
}