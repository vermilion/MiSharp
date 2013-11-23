using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;
using DotLastFm.Api.Rest;
using FileStorage.Exceptions;
using FileStorage.Helper;

namespace MiSharp.Core.Repository.FileStorage
{
    public abstract class CoverRepository : FileStorageRepository
    {
        private readonly BitmapImage _defaultCover =
            new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/Disc.ico"));

        protected CoverRepository(string path)
            : base(path)
        {
        }

        public BitmapImage GetCover(Guid guid, string artist, string album = null)
        {
            var item = new byte[] { };

            try
            {
                item = Get(guid);
            }
            catch (DataIdentifierNotFoundException)
            {
                if (Settings.Instance.CoverDownloadEnabled)
                {
                    item = DownloadImage(guid, artist, album, 5);
                }
            }

            if (item.Length == 0) return _defaultCover;

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

        protected virtual string DownloadImageImpl(string artist, string album)
        {
            throw new Exception("Invalid call");
        }

        protected byte[] DownloadImage(Guid guid, string artist, string album, int triesLeft)
        {
            var item = new byte[] {};

            try
            {
                var url = DownloadImageImpl(artist, album);
                if (url != null)
                {
                    item = ResizeImageByUrl(url, 300);
                    StoreBytes(guid, item);
                }

            }
            catch (LastFmApiException)
            {
                if (triesLeft > 0)
                {
                    Thread.Sleep(5000);
                    DownloadImage(guid, artist, album, --triesLeft);
                }
            }

            return item;
        }

        protected byte[] ResizeImageByUrl(string url, int largestSide)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            // execute the web request
            var response = (HttpWebResponse)request.GetResponse();

            byte[] thumbnail;
            using (Stream webStream = response.GetResponseStream())
            {
                thumbnail = ImageHelper.CreateThumbnail(webStream.ReadAllBytes(), largestSide);
            }
            return thumbnail;
        }
    }
}