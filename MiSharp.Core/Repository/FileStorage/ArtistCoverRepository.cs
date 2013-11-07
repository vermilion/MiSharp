using System;
using System.IO;
using System.Windows.Media.Imaging;
using FileStorage.Exceptions;

namespace MiSharp.Core.Repository.FileStorage
{
    public class ArtistCoverRepository : FileStorageRepository
    {
        private const string Name = "ArtistCovers";

        private static ArtistCoverRepository _instance;

        public ArtistCoverRepository()
            : base(Name)
        {
        }

        public static ArtistCoverRepository Instance
        {
            get { return _instance ?? (_instance = new ArtistCoverRepository()); }
        }


        public BitmapImage GetCover(string key, Guid guid)
        {
            var item = new byte[] {};
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