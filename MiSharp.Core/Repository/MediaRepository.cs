using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using DeadDog.Audio.Libraries;
using System.IO;
using DotLastFm;
using DotLastFm.Api;
using Splat;

namespace MiSharp.Core.Repository
{
    public class MediaRepository : RepositoryBase
    {
        private const string LibPath = "library.lib";
        private static MediaRepository _instance;

        public MediaRepository() : base(LibPath)
        {
        }

        public static MediaRepository Instance
        {
            get { return _instance ?? (_instance = new MediaRepository()); }
        }

        public Library GetLibrary()
        {
            return Repository.GetAll<Library>().FirstOrDefault() ?? new Library();
        }
    }

    public class CoverRepository : RepositoryBase
    {
        private const string LibPath = "covers.lib";
        private static CoverRepository _instance;
        private static readonly object SyncRoot = new Object();

        public CoverRepository()
            : base(LibPath)
        {
        }

        public static CoverRepository Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CoverRepository();
                }

                return _instance;
            }
        }

        public BitmapSource GetArtistCover(string name)
        {
            var res = Repository.Container.Query<CoverIdentity>(x => x.Name == name);
            if (res.Count == 1)
                return LoadImage(res.First().ImageSrc);

            var url = LoadArtistCover(name);
            if (url != null)
            {
                var wc = new WebClient();
                var imageBytes = wc.DownloadData(url);

                var identity = new CoverIdentity() { ImageSrc = imageBytes, Name = name };
                Repository.SaveSync(identity);
                return LoadImage(imageBytes);
            }
            return null;
        }

        public BitmapSource GetAlbumCover(string album, string artist)
        {
            var res = Repository.Container.Query<CoverIdentity>(x => x.Name == album);//TODO:fix
            if (res.Count == 1)
                return LoadImage(res.First().ImageSrc);

            var url = LoadAlbumCover(album, artist);
            if (url != null)
            {
                var wc = new WebClient();
                var imageBytes = wc.DownloadData(url);

                var identity = new CoverIdentity() { ImageSrc = imageBytes, Name = album };//TODO:album/artist
                Repository.SaveSync(identity);
                return LoadImage(imageBytes);
            }
            return null;
        }

        private BitmapSource LoadImage(byte[] imageData)
        {
            var ms = new MemoryStream(imageData);
            IBitmap bitmap = BitmapLoader.Current.Load(ms, null, null).Result;
            
            return bitmap.ToNative();
        }

        private string LoadArtistCover(string name)
        {
            var api = new LastFmApi(new TestLastFmConfig());
            var result=api.Artist.GetInfo(name);
            if (result != null)
            {
                if (result.Images.Count > 0)
                {
                    var url = result.Images.Last().Value;
                    if (string.IsNullOrEmpty(url)) return null;
                    return url;
                }
            }
            return null;
        }

        private string LoadAlbumCover(string album, string artist)
        {
            var api = new LastFmApi(new TestLastFmConfig());
            var result = api.Album.GetInfo(album, artist);
            if (result != null)
            {
                if (result.Images.Count > 0)
                {
                    var url = result.Images.Last().Value;
                    if(string.IsNullOrEmpty(url)) return null;
                    return url;
                }
            }
            return null;
        }


        public class TestLastFmConfig : ILastFmConfig
        {
            /// <summary>
            /// Gets the base Last.fm's URL.
            /// </summary>
            public string BaseUrl
            {
                get
                {
                    return "http://ws.audioscrobbler.com/2.0";
                }
            }

            /// <summary>
            /// Gets the Last.fm's API key.
            /// </summary>
            public string ApiKey
            {
                get
                {
                    return "ae66f34b0428ef4cf946b522e6a83776";
                }
            }

            /// <summary>
            /// Gets the Last.fm's secret.
            /// </summary>
            public string Secret
            {
                get
                {
                    return "-- NOT USED --";
                }
            }
        }

        public void SetCover(string coverName, BitmapSource image)
        {
            //var item = new CoverIdentity() { Image = image, Name = coverName };  
            //var res = Repository.Container.Query<CoverIdentity>(x => x.Name == coverName);
            //if (res.Count == 1)
            //{
            //    item = res.First();
            //    item.Image = image;
            //    item.Name = coverName;
            //}
            //Repository.Save(item);
        }

        public class CoverIdentity
        {
            public string Name { get; set; }
            public byte[] ImageSrc { get; set; }
        }
    }
}