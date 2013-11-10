using System;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using FileStorage.Exceptions;

namespace MiSharp.Core.Repository.FileStorage
{
    public class AlbumCoverRepository : FileStorageRepository
    {
        private const string Name = "AlbumCovers";

        private static readonly SemaphoreSlim Locker;

        static AlbumCoverRepository()
        {
            Locker = new SemaphoreSlim(1, 1);
        }

        public AlbumCoverRepository()
            : base(Name)
        {
        }

        public BitmapImage GetCover(string key, string artist, Guid guid)
        {
            var item = new byte[] {};
            Locker.Wait();

            try
            {
                item = Get(guid);
            }
            catch (DataIdentifierNotFoundException)
            {
                //try download
                string url = Api.LoadAlbumCover(key, artist);
                if (url != null)
                {
                    StoreAndResizeImageByUrl(url, 300, guid);
                    item = Get(guid);
                }
            }

            Locker.Release();

            if (item.Length == 0) return null;

            var source = new BitmapImage();
            using (Stream stream = new MemoryStream(item))
            {
                source.BeginInit();
                source.StreamSource = stream;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
            }
            source.Freeze();
            return source;
        }
    }
}