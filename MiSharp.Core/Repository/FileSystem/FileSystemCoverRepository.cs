using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MiSharp.Core.Repository.FileSystem
{
    public abstract class FileSystemCoverRepository
    {
        protected readonly LastfmApiWrapper Api;
        protected string CoversPath;
        protected Dictionary<string, FileInfo> NameCoverDictionary;

        protected FileSystemCoverRepository()
        {
            Api = new LastfmApiWrapper();
        }

        protected void InitDictionary()
        {
            NameCoverDictionary = new Dictionary<string, FileInfo>();
            foreach (FileInfo fileInfo in new DirectoryInfo(CoversPath).GetFiles())
            {
                NameCoverDictionary.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), fileInfo);
            }
        }

        protected async Task<BitmapImage> LoadFromWeb(string url, string key)
        {
            var wc = new WebClient();
            byte[] imageBytes = wc.DownloadData(url);

            var ms = new MemoryStream(imageBytes);
            using (FileStream stream = File.OpenWrite(Path.Combine(CoversPath, key + ".jpg")))
            {
                ms.CopyTo(stream);
                stream.Flush(true);
            }

            return await LoadFromDisk(key);
        }

        protected async Task<BitmapImage> LoadFromDisk(string key)
        {
            var source = new BitmapImage();
            using (Stream stream = new FileStream(NameCoverDictionary[key].FullName, FileMode.Open))
            {
                source.BeginInit();
                source.StreamSource = stream;
                source.DecodePixelWidth = 300;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
            }
            source.Freeze();
            return source;
        }
    }
}