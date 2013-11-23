using System;
using FileStorage;
using FileStorage.Enums.Behaviours;

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

        public void StoreBytes(Guid uniqueIdentifier, byte[] data)
        {
            FileStorageFacade.StoreBytes(FileStorageName, uniqueIdentifier, data, null, AddFileBehaviour.OverrideWhenAlreadyExists);
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