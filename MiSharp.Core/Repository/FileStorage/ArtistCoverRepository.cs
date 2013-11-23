using System;

namespace MiSharp.Core.Repository.FileStorage
{
    public class ArtistCoverRepository : CoverRepository
    {
        private const string Name = "ArtistCovers";

        public ArtistCoverRepository()
            : base(Name)
        {
        }

        protected override string DownloadImageImpl(string artist, string album)
        {
            try
            {
                return Api.LoadArtistCover(artist);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}