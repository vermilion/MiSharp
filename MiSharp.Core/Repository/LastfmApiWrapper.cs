using System.Linq;
using DotLastFm;
using DotLastFm.Api;
using DotLastFm.Models;

namespace MiSharp.Core.Repository
{
    public class LastfmApiWrapper
    {
        private readonly LastFmApi _api;

        public LastfmApiWrapper()
        {
            _api = new LastFmApi(new TestLastFmConfig());
        }


        public string LoadArtistCover(string name)
        {
            ArtistWithDetails result = _api.Artist.GetInfo(name);
            if (result != null)
            {
                if (result.Images.Count > 0)
                {
                    string url = result.Images.Last().Value;
                    return string.IsNullOrEmpty(url) ? null : url;
                }
            }
            return null;
        }

        public string LoadAlbumCover(string album, string artist)
        {
            AlbumWithDetails result = _api.Album.GetInfo(album, artist);
            if (result != null)
            {
                if (result.Images.Count > 0)
                {
                    string url = result.Images.Last().Value;
                    return string.IsNullOrEmpty(url) ? null : url;
                }
            }
            return null;
        }

        public class TestLastFmConfig : ILastFmConfig
        {
            /// <summary>
            ///     Gets the base Last.fm's URL.
            /// </summary>
            public string BaseUrl
            {
                get { return "http://ws.audioscrobbler.com/2.0"; }
            }

            /// <summary>
            ///     Gets the Last.fm's API key.
            /// </summary>
            public string ApiKey
            {
                get { return "ae66f34b0428ef4cf946b522e6a83776"; }
            }

            /// <summary>
            ///     Gets the Last.fm's secret.
            /// </summary>
            public string Secret
            {
                get { return "-- NOT USED --"; }
            }
        }
    }
}