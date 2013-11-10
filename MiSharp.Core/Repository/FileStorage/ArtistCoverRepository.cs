using System;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using FileStorage.Exceptions;

namespace MiSharp.Core.Repository.FileStorage
{
    public class ArtistCoverRepository : FileStorageRepository
    {
        private const string Name = "ArtistCovers";

        private static readonly SemaphoreSlim Locker;

        static ArtistCoverRepository()
        {
            Locker = new SemaphoreSlim(1,1);
        }

        public ArtistCoverRepository()
            : base(Name)
        {
        }

        public BitmapImage GetCover(string key, Guid guid)
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
                string url = Api.LoadArtistCover(key);
                if (url != null)
                {                    
                    StoreAndResizeImageByUrl(url, 150, guid);                    
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