using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DynamiteXml;
using FileStorage.Enums.Behaviours;
using FileStorage.Enums.Features;
using FileStorage.Factories;
using FileStorage.Handler;
using FileStorage.MetaData;
using FileStorage.MetaData.Helper;

namespace FileStorage
{
    /// <summary>
    ///     See http://www.codeplex.com/NFileStorage
    /// </summary>
    public class FileStorageFacade
    {
        #region Create, restore, replicate, defrag functionality

        public static void Create(string fileStorageName, CreateFileStorageBehaviour createFileStorageBehaviour)
        {
            FileStorageHandler.Create(fileStorageName, FileStorageFeatureFactory.GetDefaultFeatures(), createFileStorageBehaviour);
        }

        public static void RestoreIndexFile(string fileStorageName, AddFileBehaviour addFileBehaviour, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegate)
        {
            FileStorageHandler.RestoreIndexFile(fileStorageName, addFileBehaviour, exposeProgressDelegate);
        }

        public static void Replicate(string sourcefileStorageName, string targetfileStorageName, ReplicateBehaviour replicateBehaviour, AddFileBehaviour addFileBehaviour, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegatePhase1, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegatePhase2)
        {
            FileStorageHandler sourceFileStorageHandler = FileStorageHandler.Open(sourcefileStorageName);

            switch (replicateBehaviour)
            {
                case ReplicateBehaviour.AddToExistingStorage:
                    //
                    // nothing to check here
                    //

                    break;
                case ReplicateBehaviour.ReplicateToNewStorage:

                    //
                    // ensure target does not yet exist
                    //
                    Create(targetfileStorageName, CreateFileStorageBehaviour.ThrowExceptionWhenExists);

                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported replicate behaviour {0}", replicateBehaviour));
            }

            FileStorageHandler targetFileStorageHandler = FileStorageHandler.Open(targetfileStorageName);

            //
            // open source streams
            //
            using (sourceFileStorageHandler.IndexStream = FileStreamFactory.CreateFileStream(sourceFileStorageHandler.IndexFilename, StreamStateBehaviour.OpenNewStreamForReading))
            {
                using (sourceFileStorageHandler.DataStream = FileStreamFactory.CreateFileStream(sourceFileStorageHandler.DataFilename, StreamStateBehaviour.OpenNewStreamForReading))
                {
                    //
                    // open target streams
                    //
                    using (targetFileStorageHandler.IndexStream = FileStreamFactory.CreateFileStream(targetFileStorageHandler.IndexFilename, StreamStateBehaviour.OpenNewStreamForReadingAndWriting))
                    {
                        using (targetFileStorageHandler.DataStream = FileStreamFactory.CreateFileStream(targetFileStorageHandler.DataFilename, StreamStateBehaviour.OpenNewStreamForReadingAndWriting))
                        {
                            List<Guid> allIdentifiers = sourceFileStorageHandler.GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour.UseExistingStream, exposeProgressDelegatePhase1);

                            //
                            // start replicate process
                            //
                            foreach (Guid dataIdentifier in allIdentifiers)
                            {
                                sourceFileStorageHandler.ExportToOtherFileStorage(dataIdentifier, targetFileStorageHandler, addFileBehaviour, StreamStateBehaviour.UseExistingStream, StreamStateBehaviour.UseExistingStream, StreamStateBehaviour.UseExistingStream, StreamStateBehaviour.UseExistingStream);
                                if (exposeProgressDelegatePhase2 != null)
                                {
                                    exposeProgressDelegatePhase2.Invoke(dataIdentifier);
                                }
                            }
                            if (exposeProgressDelegatePhase2 != null)
                            {
                                exposeProgressDelegatePhase2.Invoke(true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Makes a clone of a source filestorage, and re-allocates the data identifiers in the destination filestorage (the first
        ///     data identifier will get 0001, the next one 0002, etc. Next to outputting a new target filestorage with new indexes,
        ///     the method also produces a sql file that should be used to update your DAL logic, since the references need to be
        ///     updated too ofcourse.
        /// </summary>
        public static void DefragDataIdentifiers(string sourceFileStorageName, string destinationFileStorageName, string sqlTable, string sqlColumn, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegateA, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegateB)
        {
            Create(destinationFileStorageName, CreateFileStorageBehaviour.ThrowExceptionWhenExists);

            string sqlOutputFileName = destinationFileStorageName + ".FileStorage.index.fc.sql";
            if (File.Exists(sqlOutputFileName))
            {
                throw new Exception(string.Format("File {0} already exists", sqlOutputFileName));
            }

            FileStorageHandler sourceFileStorageHandler = FileStorageHandler.Open(sourceFileStorageName);
            FileStorageHandler destinationFileStorageHandler = FileStorageHandler.Open(destinationFileStorageName);

            using (StreamWriter sw = File.CreateText(sqlOutputFileName))
            {
                sw.WriteLine("-- SQL Patch script to adjust the dataidentifiers");

                List<Guid> dataIdentifiers = sourceFileStorageHandler.GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour.OpenNewStreamForReading, exposeProgressDelegateA);

                for (int currentDataIdentifierIndex = 0; currentDataIdentifierIndex < dataIdentifiers.Count; currentDataIdentifierIndex++)
                {
                    Guid currentSourceDataIdentifier = dataIdentifiers[currentDataIdentifierIndex];
                    byte[] theBytes = sourceFileStorageHandler.GetFileByteData(currentSourceDataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);

                    ICustomMetaData customMetaData = sourceFileStorageHandler.SupportsFeature(FileStorageFeatureEnum.StoreMetaData)
                                                         ? sourceFileStorageHandler.GetMetaDataContainer(currentSourceDataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading).CustomMetaData
                                                         : new EmptyCustomMetaData();

                    //
                    // determine the new identifier
                    //
                    int b1AsInt = (currentDataIdentifierIndex >> 24) & 255;
                    var b1 = (byte) b1AsInt;
                    int b2AsInt = (currentDataIdentifierIndex >> 16) & 255;
                    var b2 = (byte) b2AsInt;
                    int b3AsInt = (currentDataIdentifierIndex >> 8) & 255;
                    var b3 = (byte) b3AsInt;
                    int b4AsInt = (currentDataIdentifierIndex >> 0) & 255;
                    var b4 = (byte) b4AsInt;
                    var currentDestinationDataIdentifier = new Guid(0, 0, 0, 0, 0, 0, 0, b1, b2, b3, b4);

                    string line = String.Format("update {0} set {1}='{2}' where {1}='{3}'", sqlTable, sqlColumn, currentSourceDataIdentifier, currentDestinationDataIdentifier);
                    sw.WriteLine(line);

                    //
                    // store item in destination
                    //
                    destinationFileStorageHandler.StoreBytes(currentDestinationDataIdentifier, theBytes, customMetaData, AddFileBehaviour.ThrowExceptionWhenAlreadyExists, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);

                    if (exposeProgressDelegateB != null)
                    {
                        exposeProgressDelegateB.Invoke(currentDestinationDataIdentifier);
                    }
                }

                sw.WriteLine("-- EOF");
            }
        }

        /// <summary>
        ///     Makes a clone of a source filestorage, and re-allocates the data identifiers in the destination filestorage (the first
        ///     data identifier will get 0001, the next one 0002, etc. Next to outputting a new target filestorage with new indexes,
        ///     the method also produces a sql file that should be used to update your DAL logic, since the references need to be
        ///     updated too ofcourse.
        /// </summary>
        public static void Upgrade(string sourceFileStorageName, string destinationFileStorageName, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegateA, FileStorageHandler.ExposeProgressDelegate exposeProgressDelegateB)
        {
            Create(destinationFileStorageName, CreateFileStorageBehaviour.ThrowExceptionWhenExists);

            FileStorageHandler sourceFileStorageHandler = FileStorageHandler.Open(sourceFileStorageName, VersionBehaviour.BypassVersionCheck);
            FileStorageHandler destinationFileStorageHandler = FileStorageHandler.Open(destinationFileStorageName);

            List<Guid> dataIdentifiers = sourceFileStorageHandler.GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour.OpenNewStreamForReading, exposeProgressDelegateA);

            for (int currentDataIdentifierIndex = 0; currentDataIdentifierIndex < dataIdentifiers.Count; currentDataIdentifierIndex++)
            {
                Guid currentSourceDataIdentifier = dataIdentifiers[currentDataIdentifierIndex];
                byte[] theBytes = sourceFileStorageHandler.GetFileByteData(currentSourceDataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);

                ICustomMetaData customMetaData = sourceFileStorageHandler.SupportsFeature(FileStorageFeatureEnum.StoreMetaData)
                                                     ? sourceFileStorageHandler.GetMetaDataContainer(currentSourceDataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading).CustomMetaData
                                                     : new EmptyCustomMetaData();

                Guid currentDestinationDataIdentifier = currentSourceDataIdentifier;

                //
                // store item in destination
                //
                destinationFileStorageHandler.StoreBytes(currentDestinationDataIdentifier, theBytes, customMetaData, AddFileBehaviour.ThrowExceptionWhenAlreadyExists, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);

                if (exposeProgressDelegateB != null)
                {
                    exposeProgressDelegateB.Invoke(currentDestinationDataIdentifier);
                }
            }
        }

        #endregion

        #region Delete functionality

        public static void DeleteDataIdentifier(string fileStorageName, Guid dataIdentifier, DeleteFileBehaviour deleteFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.DeleteDataIdentifier(dataIdentifier, deleteFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        #endregion

        #region Retrieve functionality

        public static bool Exists(string fileStorageName, Guid dataIdentifier)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.Exists(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading);
        }

        public static void ExportToFile(string fileStorageName, Guid dataIdentifier, string outputFile, ExportFileBehaviour exportFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.ExportToFile(dataIdentifier, outputFile, exportFileBehaviour);
        }

        public static IEnumerable GetAllDataIdentifierEnumerableBasedUponFileStorageDataFile(string fileStorageName)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetAllDataIdentifierEnumerableBasedUponFileStorageDataFile();
        }

        public static int FileCountBasedUponFileStorageDataFile(string fileStorageName)
        {
            return GetAllDataIdentifierEnumerableBasedUponFileStorageDataFile(fileStorageName).Cast<Guid>().Count();
        }

        public static int FileCountBasedUponFileStorageIndexFile(string fileStorageName)
        {
            return GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName).Count;
        }

        public static byte[] GetFileByteData(string fileStorageName, Guid dataIdentifier)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetFileByteData(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
        }

        public static List<Guid> GetAllDataIdentifiersBasedUponFileStorageIndexFile(string fileStorageName)
        {
            return GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName, null);
        }

        public static List<Guid> GetAllDataIdentifiersBasedUponFileStorageIndexFile(string fileStorageName, FileStorageHandler.ExposeProgressDelegate notificationDelegate)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour.OpenNewStreamForReading, notificationDelegate);
        }

        public static void ExportToOtherFileStorage(string sourcefileStorageName, Guid dataIdentifier, AddFileBehaviour addFileBehaviour, string targetfileStorageName)
        {
            FileStorageHandler sourceFileStorageHandler = FileStorageHandler.Open(sourcefileStorageName);
            FileStorageHandler targetFileStorageHandler = FileStorageHandler.Open(targetfileStorageName);
            sourceFileStorageHandler.ExportToOtherFileStorage(dataIdentifier, targetFileStorageHandler, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static string GetInfo(string fileStorageName)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(String.Format("------"));
            stringBuilder.AppendLine(String.Format("Index file {0} ", fileStorageHandler.IndexFilename));
            stringBuilder.AppendLine(String.Format("Data file {0} ", fileStorageHandler.DataFilename));
            stringBuilder.AppendLine(String.Format("Contains {0} unique files (index entries)", FileCountBasedUponFileStorageIndexFile(fileStorageName)));
            stringBuilder.AppendLine(String.Format("Contains {0} data files (data entries)", FileCountBasedUponFileStorageDataFile(fileStorageName)));
            stringBuilder.AppendLine(String.Format("------"));
            return stringBuilder.ToString();
        }

        public static string GetStringData(string fileStorageName, Guid dataIdentifier)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetStringData(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
        }

        public static string GetStringData(string fileStorageName, Guid dataIdentifier, Encoding encoding)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetStringData(dataIdentifier, encoding, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
        }

        public static IDynamiteXml GetObjectData(string fileStorageName, Guid dataIdentifier)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetObjectData(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
        }

        public static MetaDataContainer GetMetaData(string fileStorageName, Guid dataIdentifier)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            return fileStorageHandler.GetMetaDataContainer(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
        }

        #endregion

        #region Store functionality

        public static void StoreBytes(string fileStorageName, Guid dataIdentifier, byte[] fileData, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreBytes(dataIdentifier, fileData, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreFile(string fileStorageName, Guid dataIdentifier, string filename, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreFile(dataIdentifier, filename, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreStream(string fileStorageName, Guid dataIdentifier, Stream streamToStore, int numberOfBytes, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreStream(dataIdentifier, streamToStore, numberOfBytes, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreString(string fileStorageName, Guid dataIdentifier, Encoding encoding, string stringToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreString(dataIdentifier, encoding, stringToStore, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreString(string fileStorageName, Guid dataIdentifier, string stringToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreString(dataIdentifier, stringToStore, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreHttpRequest(string fileStorageName, Guid dataIdentifier, string url, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, string userAgent)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreHttpRequest(dataIdentifier, url, customMetaData, addFileBehaviour, userAgent, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        public static void StoreObject(string fileStorageName, Guid dataIdentifier, IDynamiteXml objectToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            FileStorageHandler fileStorageHandler = FileStorageHandler.Open(fileStorageName);
            fileStorageHandler.StoreObject(dataIdentifier, objectToStore, customMetaData, addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReadingAndWriting);
        }

        #endregion
    }
}