using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;
using FileStorage;
using FileStorage.Enums.Behaviours;
using FileStorage.Helper;

namespace MiSharp.Core.Repository.FileStorage
{
    public abstract class FileStorageRepository
    {
        protected readonly LastfmApiWrapper Api;
        protected string FileStorageName;

        protected FileStorageRepository(string path)
        {
            FileStorageName = path;
            FileStorageFacade.Create(FileStorageName, CreateFileStorageBehaviour.IgnoreWhenExists);
            Api = new LastfmApiWrapper();
        }


        public void StoreFile(string filename, Guid uniqueIdentifier)
        {
            FileStorageFacade.StoreFile(FileStorageName, uniqueIdentifier, filename, null, AddFileBehaviour.OverrideWhenAlreadyExists);
        }

        public void StoreFileByUrl(string url, Guid uniqueIdentifier)
        {            
            FileStorageFacade.StoreHttpRequest(FileStorageName, uniqueIdentifier, url, null, AddFileBehaviour.OverrideWhenAlreadyExists, "NFileStorage");
        }

        public void StoreAndResizeImageByUrl(string url, int largestSide, Guid uniqueIdentifier)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            // execute the web request
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream webStream = response.GetResponseStream())
            {
                var thumbnail = ImageHelper.CreateThumbnail(webStream.ReadAllBytes(), largestSide);
                FileStorageFacade.StoreBytes(FileStorageName, uniqueIdentifier, thumbnail, null, AddFileBehaviour.OverrideWhenAlreadyExists);
            }
        }

        public void DeleteAllFilesFromContainer()
        {
            foreach (Guid dataIdentifier in FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(FileStorageName))
            {
                FileStorageFacade.DeleteDataIdentifier(FileStorageName, dataIdentifier, DeleteFileBehaviour.IgnoreWhenNotExists);
            }
        }

        public byte[] Get(Guid uniqueIdentifier)
        {
            return FileStorageFacade.GetFileByteData(FileStorageName, uniqueIdentifier);
        }
    }
}