using System;
using System.IO;
using System.Windows.Media.Imaging;
using FileStorage.Exceptions;

namespace MiSharp.Core.Repository.FileStorage
{
    public class AlbumCoverRepository : FileStorageRepository
    {
        private const string Name = "AlbumCovers";

        private static AlbumCoverRepository _instance;

        public AlbumCoverRepository()
            : base(Name)
        {
        }

        public static AlbumCoverRepository Instance
        {
            get { return _instance ?? (_instance = new AlbumCoverRepository()); }
        }

        public BitmapImage GetCover(string key, string artist, Guid guid)
        {
            var item = new byte[] {};
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
                    StoreFileByUrl(url, guid);
                    item = Get(guid);
                }
            }

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