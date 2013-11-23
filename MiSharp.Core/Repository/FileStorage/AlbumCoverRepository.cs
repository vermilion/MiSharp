using System;

namespace MiSharp.Core.Repository.FileStorage
{
    public class AlbumCoverRepository : CoverRepository
    {
        private const string Name = "AlbumCovers";

        public AlbumCoverRepository()
            : base(Name)
        {
        }

        protected override string DownloadImageImpl(string artist, string album)
        {
            try
            {
                return Api.LoadAlbumCover(album, artist);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}