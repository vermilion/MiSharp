using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DynamiteXml;
using FileStorage.Enums.Behaviours;
using FileStorage.Enums.Features;
using FileStorage.Exceptions;
using FileStorage.Factories;
using FileStorage.Helper;
using FileStorage.MetaData;
using FileStorage.MetaData.Helper;
using FileStorage.Structure;

namespace FileStorage.Handler
{
    /// <summary>
    ///     Handler that is responsible for interacting with both the index and the data file on the file system that together form
    ///     the FileStorage.
    /// </summary>
    public class FileStorageHandler
    {
        public delegate void ExposeProgressDelegate(object info);

        #region Private constructor

        /// <summary>
        ///     Private constructor; its highly recommended to use the static methods and functions of the
        ///     Facade rather than to access the functionality of the handler yourself.
        ///     If you do want to access the FileStorageHandler yourself, use the static method
        ///     CreateFileStorageHandlerInstance that will return the instance for you. Please be aware of
        ///     the handling and de-allocation of the file streams yourself!
        /// </summary>
        private FileStorageHandler(string fileStorageName, VersionBehaviour versionBehaviour)
        {
            FileStorageName = fileStorageName;
            IndexFilename = FilenameFactory.GetFileStorageIndexFilename(fileStorageName);
            DataFilename = FilenameFactory.GetFileStorageDataFilename(fileStorageName);

            if (!File.Exists(IndexFilename))
            {
                throw new FileNotFoundException(string.Format("IndexFilename {0} was not found", IndexFilename));
            }
            if (!File.Exists(DataFilename))
            {
                throw new FileNotFoundException(string.Format("DataFilename {0} was not found", DataFilename));
            }

            LoadDataFileHeader();

            switch (versionBehaviour)
            {
                case VersionBehaviour.ValidateVersion:

                    if (_dataFileHeaderStruct.VersionMajor < VersionMajor)
                    {
                        throw new NotSupportedException(string.Format("Version {2} conflict (file has major version number {0}, the code expects major version {1}). Please upgrade the container using the command tool; filestoragecmd upgrade old new", _dataFileHeaderStruct.VersionMajor, VersionMajor, GetVersion()));
                    }
                    if (_dataFileHeaderStruct.VersionMajor == VersionMajor)
                    {
                        //
                        // check minor number
                        //
                        if (_dataFileHeaderStruct.VersionMinor < VersionMinor)
                        {
                            throw new NotSupportedException(string.Format("Version {2} conflict (file has minor version number {0}, the code expects minor version {1}). Please upgrade the container using the command tool; filestoragecmd upgrade old new", _dataFileHeaderStruct.VersionMinor, VersionMinor, GetVersion()));
                        }
                        if (_dataFileHeaderStruct.VersionMinor == VersionMinor)
                        {
                            //
                            // ok, its supported
                            //
                        }
                        else
                        {
                            //
                            // we cannot be sure if we are compatible, throw an exception
                            //
                            throw new NotSupportedException(string.Format("The storage was built using a more recent version ({0}) and is therefore not supported. Please upgrade your NFileStorage dll's (http://nfilestorage.codeplex.com", GetVersion()));
                        }
                    }
                    else
                    {
                        //
                        // we cannot be sure if we are compatible, throw an exception
                        //
                        throw new NotSupportedException(string.Format("The storage was built using a more recent version ({0}) and is therefore not supported. Please upgrade your NFileStorage dll's (http://nfilestorage.codeplex.com", GetVersion()));
                    }
                    break;
                case VersionBehaviour.BypassVersionCheck:
                    //
                    // no version checking needs to occur nothing to do here
                    // (this is used for example when upgrading a filestorage
                    // to a more recent version)
                    //
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported versionbehaviour {0}", versionBehaviour));
            }
        }

        #endregion

        #region Static entry points

        /// <summary>
        ///     Returns an instance of the FileStorageHandler; use the static methods and functions of the
        ///     Facade to access the 'core' functionality of the handler. Only use the instance if
        ///     you want to control the opening and closing of index and data file streams yourself (for
        ///     performance improvements).
        ///     Its is recommended to use the static methods and functions to prevent leaving file streams
        ///     open.
        /// </summary>
        public static FileStorageHandler Open(string fileStorageName)
        {
            return new FileStorageHandler(fileStorageName, VersionBehaviour.ValidateVersion);
        }

        /// <summary>
        ///     Returns an instance of the FileStorageHandler; use the static methods and functions of the
        ///     Facade to access the 'core' functionality of the handler. Only use the instance if
        ///     you want to control the opening and closing of index and data file streams yourself (for
        ///     performance improvements).
        ///     Its is recommended to use the static methods and functions to prevent leaving file streams
        ///     open.
        /// </summary>
        public static FileStorageHandler Open(string fileStorageName, VersionBehaviour versionBehaviour)
        {
            return new FileStorageHandler(fileStorageName, versionBehaviour);
        }


        public static void Create(string fileStorageName, List<FileStorageFeatureEnum> fileStorageFeatures, CreateFileStorageBehaviour createFileStorageBehaviour)
        {
            string indexFilename = FilenameFactory.GetFileStorageIndexFilename(fileStorageName);
            string dataFilename = FilenameFactory.GetFileStorageDataFilename(fileStorageName);

            switch (createFileStorageBehaviour)
            {
                case CreateFileStorageBehaviour.IgnoreWhenExists:
                    if (!File.Exists(indexFilename))
                    {
                        CreateNewFileStorage_IndexFile(indexFilename);
                    }
                    if (!File.Exists(dataFilename))
                    {
                        CreateNewFileStorage_DataFile(dataFilename, fileStorageFeatures);
                    }
                    break;
                case CreateFileStorageBehaviour.ThrowExceptionWhenExists:
                    if (File.Exists(indexFilename))
                    {
                        throw new Exception(string.Format("Index file {0} already exists", indexFilename));
                    }
                    if (File.Exists(dataFilename))
                    {
                        throw new Exception(string.Format("Data file {0} already exists", dataFilename));
                    }
                    CreateNewFileStorage_IndexFile(indexFilename);
                    CreateNewFileStorage_DataFile(dataFilename, fileStorageFeatures);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported creation behaviour {0}", createFileStorageBehaviour));
            }
        }

        #endregion

        private const int VersionMajor = 1;
        private const int VersionMinor = 3;
        public const string DllRevision = "1.3.0.0";

        private const long OffsetFirstIndexBlock = 100;
        private const long OffsetFirstDataBlock = 100;
        private const long DataFileDataStructSize = 16 + 4 + 8 + 8 + 8;
        public readonly string DataFilename;
        public readonly string FileStorageName;
        public readonly string IndexFilename;

        private DataFileHeaderStruct _dataFileHeaderStruct;
        public FileStream DataStream;
        public FileStream IndexStream;

        public DataFileHeaderStruct DataFileHeaderStruct
        {
            get { return _dataFileHeaderStruct; }
        }

        public bool SupportsFeature(FileStorageFeatureEnum fileStorageFeature)
        {
            bool result;
            if (DataFileHeaderStruct.VersionMajor == 1 && DataFileHeaderStruct.VersionMinor == 0)
            {
                //
                // in v 1.0 the feature set was not filled with 0's, so we manually override the features list for these filestorages.
                //
                result = false;
            }
            else
            {
                result = FileStorageFeatureFactory.CreateFileStorageFeatureList(DataFileHeaderStruct.FileStorageFeatures).Contains(fileStorageFeature);
            }
            return result;
        }

        private static void CreateNewFileStorage_IndexFile(string indexFilename)
        {
            using (FileStream indexStream = File.Create(indexFilename))
            {
                #region store the header

                var indexFileHeaderStruct = new IndexFileHeaderStruct
                    {
                        VersionMajor = VersionMajor,
                        VersionMinor = VersionMinor
                    };

                string template = "[IndexFile of FileStorage. Check out www.constantum.com]";
                template = template.PadRight(92, '-');
                indexFileHeaderStruct.Text = template.ToCharArray();

                #region store header

                // point to the beginning of the file
                indexStream.Seek(0, SeekOrigin.Begin);
                // write the struct

                //
                // the stream is intentionally left open
                //
                var binaryWriter = new BinaryWriter(indexStream);
                binaryWriter.Write(indexFileHeaderStruct.VersionMajor);
                binaryWriter.Write(indexFileHeaderStruct.VersionMinor);
                binaryWriter.Write(indexFileHeaderStruct.Text);

                #endregion

                #endregion

                #region store the initial index table

                const long offset = OffsetFirstIndexBlock;

                // create a new struct with offsets 0 for each index in guid
                var newIndexStruct = new AllocationTableStruct
                    {
                        Pointers = new Int64[256]
                    };

                //
                // re-open the file to allow accessing the newly added bytes
                // 
                // point to the end of the file
                indexStream.Seek(offset, SeekOrigin.Begin);
                // write the struct
                foreach (Int64 c in newIndexStruct.Pointers)
                {
                    binaryWriter.Write(c);
                }

                #endregion
            }
        }

        private static void CreateNewFileStorage_DataFile(string dataFilename, List<FileStorageFeatureEnum> fileStorageFeatures)
        {
            if (File.Exists(dataFilename))
            {
                throw new NotSupportedException(string.Format("File {0} already exists", dataFilename));
            }

            #region store the header

            int fileStorageFeaturesValue = FileStorageFeatureFactory.CreateFileStorageFeatureValue(fileStorageFeatures);
            string template = "[FileStorage datafile. Check out www.constantum.com]";
            //
            // fill up the template to end up with 100 bytes header
            //
            template = template.PadRight(88, '-');

            using (FileStream dataStream = File.Create(dataFilename))
            {
                dataStream.Seek(0, SeekOrigin.Begin);
                var dataFileHeaderStruct = new DataFileHeaderStruct
                    {
                        VersionMajor = VersionMajor,
                        VersionMinor = VersionMinor,
                        FileStorageFeatures = fileStorageFeaturesValue,
                        Text = template.ToCharArray()
                    };

                // point to the end of the file
                dataStream.Seek(0, SeekOrigin.Begin);
                // write the struct

                //
                // the stream is intentionally left open
                //
                var binaryWriter = new BinaryWriter(dataStream);
                binaryWriter.Write(dataFileHeaderStruct.VersionMajor); // 4
                binaryWriter.Write(dataFileHeaderStruct.VersionMinor); // 4
                binaryWriter.Write(dataFileHeaderStruct.FileStorageFeatures); // 4
                binaryWriter.Write(dataFileHeaderStruct.Text); // 100 - others (88 in this case)
            }

            #endregion
        }

        public static void RestoreIndexFile(string fileStorageName, AddFileBehaviour addFileBehaviour, ExposeProgressDelegate exposeProgressDelegate)
        {
            //
            // the following method also checks whether the index file exists or not,
            // if exists, it throws an exception
            //
            CreateNewFileStorage_IndexFile(FilenameFactory.GetFileStorageIndexFilename(fileStorageName));

            var fileStorageHandler = new FileStorageHandler(fileStorageName, VersionBehaviour.ValidateVersion);
            fileStorageHandler.RestoreIndexFile(addFileBehaviour, StreamStateBehaviour.OpenNewStreamForReadingAndWriting, StreamStateBehaviour.OpenNewStreamForReading, exposeProgressDelegate);
        }

        public void RestoreIndexFile(AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour, ExposeProgressDelegate exposeProgressDelegate)
        {
            //
            // the following method also checks whether the index file exists or not,
            // if exists, it throws an exception
            //
            CreateNewFileStorage_IndexFile(IndexFilename);

            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReadingAndWriting:
                    // ok
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }

            switch (dataStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReading:
                    // ok
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected dataStreamStateBehaviour {0}", dataStreamStateBehaviour));
            }

            using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, StreamStateBehaviour.OpenNewStreamForReadingAndWriting))
            {
                using (DataStream = FileStreamFactory.CreateFileStream(DataFilename, StreamStateBehaviour.OpenNewStreamForReading))
                {
                    long dataSize = DataStream.Length;
                    long offset = OffsetFirstDataBlock;

                    while (true)
                    {
                        InterpretedDataFileDataStruct retrieveFileStruct = GetInterpretedDataStructUsingExistingStream(offset);

                        //
                        // the offset in the index file should point to the starting location 
                        // of the data struct, this is not the offset where the actual data of
                        // the file is stored, but the start of the struct before the actual data.
                        //

                        Guid dataIdentifier = retrieveFileStruct.DataIdentification;

                        //
                        // if the data file contains duplicate dataIdentifiers, we will override previous ones (which
                        // is acceptable, as new files are appeneded to the data file, so more up to date files are stored
                        // after older ones).
                        //
                        BuildIndex(dataIdentifier, offset, BuildIndexBehaviour.OverrideWhenAlreadyExists, StreamStateBehaviour.UseExistingStream);

                        //
                        // jump to the next data block
                        //
                        offset = retrieveFileStruct.BinaryDataOffset + retrieveFileStruct.BinaryDataSizeInBytes;

                        //
                        // inform the outside world about the progress we've made,
                        // in case the outside world is interested
                        //
                        if (exposeProgressDelegate != null)
                        {
                            exposeProgressDelegate.Invoke(dataIdentifier);
                        }

                        if (offset >= dataSize)
                        {
                            //
                            // reach the end of the file
                            //
                            break;
                        }
                    }
                }
            }

            if (exposeProgressDelegate != null)
            {
                exposeProgressDelegate.Invoke(true);
            }
        }

        #region Store functionality

        private void BuildIndex(Guid dataIdentifier, int currentIndexInGuid, Int64 currentOffset, Int64 dataOffset, BuildIndexBehaviour buildIndexBehaviour, StreamStateBehaviour indexStreamStateBehaviour)
        {
            byte[] bytes = dataIdentifier.ToByteArray();
            int indexerInGuid = bytes[currentIndexInGuid];
            Int64 endOfFileOffset = IndexStream.Length;

            AllocationTableStruct currentIndexStruct = GetIndexStruct(currentOffset, indexStreamStateBehaviour);

            if (currentIndexInGuid < 15)
            {
                long nextOffset = currentIndexStruct.Pointers[indexerInGuid];

                if (nextOffset == 0)
                {
                    //
                    // does not point to anywhere; this means we have to;
                    // * append a new block at the end of the file, with 0-pointers for each guid indexer over there
                    // * update the reference for the specific index in the guid to the end of the filestream
                    //

                    // update the struct
                    currentIndexStruct.Pointers[indexerInGuid] = endOfFileOffset;
                    // write the struct
                    StoreStruct(currentOffset, currentIndexStruct);

                    // create a new struct with offsets 0 for each index in guid
                    var newIndexStruct = new AllocationTableStruct
                        {
                            Pointers = new Int64[256]
                        };

                    StoreStruct(endOfFileOffset, newIndexStruct);

                    nextOffset = endOfFileOffset;

                    //
                    // recursively call BuildIndex for the next index in the guid
                    //
                    BuildIndex(dataIdentifier, currentIndexInGuid + 1, nextOffset, dataOffset, buildIndexBehaviour, indexStreamStateBehaviour);
                }
                else
                {
                    //
                    // recursively call BuildIndex for the next index in the guid
                    //
                    BuildIndex(dataIdentifier, currentIndexInGuid + 1, nextOffset, dataOffset, buildIndexBehaviour, indexStreamStateBehaviour);
                }
            }
            else if (currentIndexInGuid == 15)
            {
                //
                // we reached the last index struct
                //

                switch (buildIndexBehaviour)
                {
                    case BuildIndexBehaviour.OverrideWhenAlreadyExists:
                        //
                        // no matter if it already existed or not, we will override it
                        //
                        break;
                    case BuildIndexBehaviour.ThrowExceptionWhenIndexAlreadyExists:
                        if (currentIndexStruct.Pointers[indexerInGuid] == 0)
                        {
                            //
                            // its still free, continue
                            //
                            break;
                        }

                        //
                        // if we reach this point, it means the pointer was already allocated
                        //
                        throw new NotSupportedException(string.Format("Index {0} is already allocated", dataIdentifier));
                    default:
                        throw new NotSupportedException(string.Format("{0} is an unsupported BuildIndexBehaviour", buildIndexBehaviour));
                }

                currentIndexStruct.Pointers[indexerInGuid] = dataOffset;
                // store the index block
                StoreStruct(currentOffset, currentIndexStruct);
            }
            else
            {
                throw new IndexOutOfRangeException(currentIndexInGuid.ToString());
            }
        }

        private void StoreStruct(Int64 offsetInFileFromBegin, AllocationTableStruct indexStruct)
        {
            //
            // re-open the file to allow accessing the newly added bytes
            // 
            // point to the end of the file
            IndexStream.Seek(offsetInFileFromBegin, SeekOrigin.Begin);
            // write the struct
            var binaryWriter = new BinaryWriter(IndexStream);
            foreach (Int64 c in indexStruct.Pointers)
            {
                binaryWriter.Write(c);
            }
        }

        private void StoreStruct(Int64 offsetInFileFromBegin, DataFileDataStruct fileStruct)
        {
            // point to the end of the file
            DataStream.Seek(offsetInFileFromBegin, SeekOrigin.Begin);
            // write the struct
            var binaryWriter = new BinaryWriter(DataStream);
            binaryWriter.Write(fileStruct.DataIdentification.ToByteArray());
            binaryWriter.Write(fileStruct.DataStateEnumID);
            binaryWriter.Write(fileStruct.BinaryDataSizeInBytes);
            binaryWriter.Write(fileStruct.MetaDataOffsetInFileFromBegin);
            binaryWriter.Write(fileStruct.ReservedB);
        }

        private void LoadDataFileHeader()
        {
            //
            // the stream will be opened and closed
            //
            using (DataStream = FileStreamFactory.CreateFileStream(DataFilename, StreamStateBehaviour.OpenNewStreamForReading))
            {
                // point to the beginning of the file
                DataStream.Seek(0, SeekOrigin.Begin);
                // read the struct

                //
                // the stream is intentionally left open
                //
                var binaryReader = new BinaryReader(DataStream);
                _dataFileHeaderStruct.VersionMajor = binaryReader.ReadInt32(); // 4
                _dataFileHeaderStruct.VersionMinor = binaryReader.ReadInt32(); // 4
                _dataFileHeaderStruct.FileStorageFeatures = binaryReader.ReadInt32(); // 4
                _dataFileHeaderStruct.Text = binaryReader.ReadChars(88); // 100 - others (88 in this case)
            }
        }

        public void StoreHttpRequest(Guid dataIdentifier, string url, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, string userAgent, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.UserAgent = userAgent;

            // execute the web request
            var response = (HttpWebResponse) request.GetResponse();

            using (Stream webStream = response.GetResponseStream())
            {
                StoreBytes(dataIdentifier, webStream.ReadAllBytes(), customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
            }
        }

        public void StoreFile(Guid dataIdentifier, string filename, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            StoreBytes(dataIdentifier, File.ReadAllBytes(filename), customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
        }

        public void StoreStream(Guid dataIdentifier, Stream streamToStore, int numberOfBytes, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            using (var binaryReader = new BinaryReader(streamToStore))
            {
                byte[] fileData = binaryReader.ReadBytes(numberOfBytes);
                StoreBytes(dataIdentifier, fileData, customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
            }
        }

        public void StoreObject(Guid dataIdentifier, IDynamiteXml objectToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            string serializedObject = DynamiteXmlLogic.Serialize(objectToStore);
            StoreString(dataIdentifier, serializedObject, customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
        }

        public void StoreBytes(Guid dataIdentifier, byte[] fileData, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReadingAndWriting:
                    //
                    // open the stream and make a recursive call
                    //
                    using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, indexStreamStateBehaviour))
                    {
                        StoreBytes(dataIdentifier, fileData, customMetaData, addFileBehaviour, StreamStateBehaviour.UseExistingStream, dataStreamStateBehaviour);
                    }
                    break;
                case StreamStateBehaviour.UseExistingStream:
                    // 
                    // check state of data file stream
                    //
                    switch (dataStreamStateBehaviour)
                    {
                        case StreamStateBehaviour.OpenNewStreamForReadingAndWriting:
                            //
                            // open the stream and make a recursive call
                            //
                            using (DataStream = FileStreamFactory.CreateFileStream(DataFilename, dataStreamStateBehaviour))
                            {
                                StoreBytes(dataIdentifier, fileData, customMetaData, addFileBehaviour, indexStreamStateBehaviour, StreamStateBehaviour.UseExistingStream);
                            }
                            break;
                        case StreamStateBehaviour.UseExistingStream:
                            //
                            //
                            //
                            StoreBytesForExistingStreams(dataIdentifier, fileData, customMetaData, addFileBehaviour);
                            //
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unexpected dataStreamStateBehaviour {0}", dataStreamStateBehaviour));
                    }
                    //
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }
        }

        public void StoreBytesForExistingStreams(Guid dataIdentifier, byte[] fileData, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour)
        {
            #region Preconditions and initialization ...

            if (fileData.LongLength > fileData.Length)
            {
                throw new Exception("NFileStorage is (currently) limited to a max length of 32bit (about 4.294.967.296 bytes)");
            }

            //
            // first check the behaviour conditions
            //
            switch (addFileBehaviour)
            {
                case AddFileBehaviour.SkipWhenAlreadyExists:
                    if (Exists(dataIdentifier, StreamStateBehaviour.UseExistingStream))
                    {
                        return;
                    }
                    break;
                case AddFileBehaviour.ThrowExceptionWhenAlreadyExists:
                    if (Exists(dataIdentifier, StreamStateBehaviour.UseExistingStream))
                    {
                        throw new Exception(string.Format("File {0} already exists", dataIdentifier));
                    }
                    break;
                case AddFileBehaviour.OverrideWhenAlreadyExists:
                    if (Exists(dataIdentifier, StreamStateBehaviour.UseExistingStream))
                    {
                        DeleteDataIdentifier(dataIdentifier, DeleteFileBehaviour.ThrowExceptionWhenNotExists,
                                             StreamStateBehaviour.UseExistingStream);
                    }
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported behaviour; {0}", addFileBehaviour));
            }

            #endregion

            bool supportsMetaData = SupportsFeature(FileStorageFeatureEnum.StoreMetaData);

            long dataOffset = DataStream.Length;
            int dataLength = fileData.Length;
            Int64 metaDataOffsetInFileFromBegin;
            if (supportsMetaData)
            {
                if (customMetaData == null)
                {
                    customMetaData = new EmptyCustomMetaData();
                }
                metaDataOffsetInFileFromBegin = dataOffset + DataFileDataStructSize + dataLength;
            }
            else
            {
                metaDataOffsetInFileFromBegin = 0;
            }

            // create a new struct with offsets 0 for each index in guid
            var newFileStruct = new DataFileDataStruct
                {
                    DataIdentification = dataIdentifier,
                    BinaryDataSizeInBytes = dataLength,
                    DataStateEnumID = 0,
                    MetaDataOffsetInFileFromBegin = metaDataOffsetInFileFromBegin,
                    ReservedB = 0
                };

            #region Store the information in the data file

            #region Store struct in data file

            StoreStruct(dataOffset, newFileStruct);

            #endregion

            #region Store actual data in data file

            //
            // the stream is intentionally left open
            //
            var bufferedDataFileStream = new BufferedStream(DataStream);
            // write the actual data
            bufferedDataFileStream.Write(fileData, 0, dataLength);
            // flush, to ensure we don't have bytes in the queue when we close the file
            bufferedDataFileStream.Flush();

            #endregion

            #region Store meta data in data file

            if (supportsMetaData)
            {
                //
                // use the DynamiteXML feature to store metadata
                //
                var metaDataContainer = new MetaDataContainer(customMetaData, DateTime.UtcNow, dataLength);
                string metaDataString = DynamiteXmlLogic.Serialize(metaDataContainer);
                byte[] metaDataBytes = Encoding.UTF8.GetBytes(metaDataString);

                //
                // write the length of the bytes of the metadata
                //
                var binaryWriter = new BinaryWriter(DataStream);
                binaryWriter.Write(metaDataBytes.Length);

                bufferedDataFileStream.Write(metaDataBytes, 0, metaDataBytes.Length);
                // flush, to ensure we don't have bytes in the queue when we close the file
                bufferedDataFileStream.Flush();
            }

            #endregion

            #endregion

            #region Store the information in the index file

            //
            // derive the build index behaviour from the add file behaviour
            //
            BuildIndexBehaviour buildIndexBehaviour;
            switch (addFileBehaviour)
            {
                case AddFileBehaviour.ThrowExceptionWhenAlreadyExists:
                    buildIndexBehaviour = BuildIndexBehaviour.ThrowExceptionWhenIndexAlreadyExists;
                    break;
                case AddFileBehaviour.SkipWhenAlreadyExists:
                    //
                    // this should have been intercepted before, should be impossible
                    //
                    buildIndexBehaviour = BuildIndexBehaviour.ThrowExceptionWhenIndexAlreadyExists;
                    break;
                case AddFileBehaviour.OverrideWhenAlreadyExists:
                    buildIndexBehaviour = BuildIndexBehaviour.OverrideWhenAlreadyExists;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected addFileBehaviour {0}", addFileBehaviour));
            }

            // we update the index file
            BuildIndex(dataIdentifier, dataOffset, buildIndexBehaviour, StreamStateBehaviour.UseExistingStream);

            #endregion
        }

        public void StoreString(Guid dataIdentifier, string stringToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            Encoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(stringToStore);

            StoreBytes(dataIdentifier, bytes, customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
        }

        public void StoreString(Guid dataIdentifier, Encoding encoding, string stringToStore, ICustomMetaData customMetaData, AddFileBehaviour addFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            Byte[] bytes = encoding.GetBytes(stringToStore);

            StoreBytes(dataIdentifier, bytes, customMetaData, addFileBehaviour, indexStreamStateBehaviour, dataStreamStateBehaviour);
        }

        public void BuildIndex(Guid dataIdentifier, Int64 dataOffset, BuildIndexBehaviour buildIndexBehaviour, StreamStateBehaviour indexStreamStateBehaviour)
        {
            int indexInGuid = 0;
            long currentOffsetInIndexFile = OffsetFirstIndexBlock;

            BuildIndex(dataIdentifier, indexInGuid, currentOffsetInIndexFile, dataOffset, buildIndexBehaviour, indexStreamStateBehaviour);
        }

        #endregion

        #region Retrieve functionality

        private InterpretedDataFileDataStruct GetInterpretedDataStructUsingExistingStream(Int64 offsetInFileFromBegin)
        {
            var result = new InterpretedDataFileDataStruct();

            DataStream.Seek(offsetInFileFromBegin, SeekOrigin.Begin);
            // write the struct
            var binaryReader = new BinaryReader(DataStream);

            result.DataIdentification = new Guid(binaryReader.ReadBytes(16));
            result.DataStateEnumID = binaryReader.ReadInt32();
            result.BinaryDataSizeInBytes = binaryReader.ReadInt64();
            result.MetaDataOffsetInFileFromBegin = binaryReader.ReadInt64();
            result.ReservedB = binaryReader.ReadInt64();

            //
            // after reading the last property, the position now points to the offset where the data starts
            //
            result.BinaryDataOffset = DataStream.Position;

            return result;
        }

        /// <summary>
        ///     Returns 0 if the dataIdentifier does not exist, or the offset of the dataIdentifier
        ///     in the datafile if it does exist.
        /// </summary>
        private Int64 GetOffset(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour)
        {
            Int64 result;

            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReading:
                    //
                    // we have to create a new one, and use recursion to get the actual return value
                    //
                    using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, indexStreamStateBehaviour))
                    {
                        result = GetOffset(dataIdentifier, StreamStateBehaviour.UseExistingStream);
                    }
                    break;
                case StreamStateBehaviour.UseExistingStream:
                    result = GetOffsetInCurrentStream(dataIdentifier, 0, OffsetFirstIndexBlock);
                    break;
                default:
                    throw new NotSupportedException(string.Format("unknown indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }

            return result;
        }

        private Int64 GetOffsetInCurrentStream(Guid dataIdentifier, int currentIndexInGuid, Int64 currentOffset)
        {
            Int64 result;

            byte[] bytes = dataIdentifier.ToByteArray();
            int indexerInGuid = bytes[currentIndexInGuid];

            var currentIndexStruct = new AllocationTableStruct
                {
                    Pointers = new Int64[256]
                };

            IndexStream.Seek(currentOffset, SeekOrigin.Begin);

            var binaryReader = new BinaryReader(IndexStream);

            for (int elementIndex = 0; elementIndex < 256; elementIndex++)
            {
                currentIndexStruct.Pointers[elementIndex] = binaryReader.ReadInt64();
            }

            if (currentIndexInGuid < 15)
            {
                long nextOffset = currentIndexStruct.Pointers[indexerInGuid];

                if (nextOffset == 0)
                {
                    //
                    // file does not exist
                    //
                    result = 0;
                }
                else
                {
                    //
                    // recursively call BuildIndex for the next index in the guid
                    //
                    result = GetOffsetInCurrentStream(dataIdentifier, currentIndexInGuid + 1, nextOffset);
                }
            }
            else if (currentIndexInGuid == 15)
            {
                //
                // we reached the last index struct
                //

                if (currentIndexStruct.Pointers[indexerInGuid] == 0)
                {
                    //
                    // file does not exist
                    //
                    result = 0;
                }
                else
                {
                    result = currentIndexStruct.Pointers[indexerInGuid];
                }
            }
            else
            {
                throw new IndexOutOfRangeException(currentIndexInGuid.ToString());
            }

            return result;
        }

        public string GetVersion()
        {
            return string.Format("{0}.{1}", _dataFileHeaderStruct.VersionMajor, _dataFileHeaderStruct.VersionMinor);
        }

        public AllocationTableStruct GetIndexStruct(Int64 currentOffset, StreamStateBehaviour indexStreamStateBehaviour)
        {
            if (indexStreamStateBehaviour != StreamStateBehaviour.UseExistingStream)
            {
                throw new NotSupportedException("Only useexistingstream is currently supported ");
            }

            var result = new AllocationTableStruct
                {
                    Pointers = new Int64[256]
                };

            IndexStream.Seek(currentOffset, SeekOrigin.Begin);

            //
            // the stream is intentionaly left open
            //
            var binaryReader = new BinaryReader(IndexStream);
            for (int elementIndex = 0; elementIndex < 256; elementIndex++)
            {
                result.Pointers[elementIndex] = binaryReader.ReadInt64();
            }

            return result;
        }

        public bool Exists(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour)
        {
            bool result;

            long offset = GetOffset(dataIdentifier, indexStreamStateBehaviour);
            if (offset == 0)
            {
                //
                // does not exist
                //
                result = false;
            }
            else
            {
                //
                // does exist
                //
                result = true;
            }

            return result;
        }

        public void ExportToFile(Guid dataIdentifier, string outputFile, ExportFileBehaviour exportFileBehaviour)
        {
            switch (exportFileBehaviour)
            {
                case ExportFileBehaviour.OverrideWhenAlreadyExists:
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }
                    break;
                case ExportFileBehaviour.SkipWhenAlreadyExists:
                    if (File.Exists(outputFile))
                    {
                        return;
                    }
                    break;
                case ExportFileBehaviour.ThrowExceptionWhenAlreadyExists:
                    if (File.Exists(outputFile))
                    {
                        throw new Exception(String.Format("File {0} already exists", outputFile));
                    }
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported export behavious ({0})", exportFileBehaviour));
            }

            byte[] fileData = GetFileByteData(dataIdentifier, StreamStateBehaviour.OpenNewStreamForReading, StreamStateBehaviour.OpenNewStreamForReading);
            File.WriteAllBytes(outputFile, fileData);
        }

        public void ExportToOtherFileStorage(Guid dataIdentifier, FileStorageHandler targetFileStorageHandler, AddFileBehaviour addFileBehaviour, StreamStateBehaviour sourceIndexStreamStateBehaviour, StreamStateBehaviour sourceDataStreamStateBehaviour, StreamStateBehaviour targetIndexStreamStateBehaviour, StreamStateBehaviour targetDataStreamStateBehaviour)
        {
            if (targetFileStorageHandler.FileStorageName.Equals(FileStorageName))
            {
                throw new NotSupportedException(string.Format("Cannot export file from a filestorage to another filestorage with the same name ({0})", FileStorageName));
            }
            byte[] fileData = GetFileByteData(dataIdentifier, sourceIndexStreamStateBehaviour, sourceDataStreamStateBehaviour);
            MetaDataContainer metaDataContainer = GetMetaDataContainer(dataIdentifier, sourceIndexStreamStateBehaviour, sourceDataStreamStateBehaviour);
            targetFileStorageHandler.StoreBytes(dataIdentifier, fileData, metaDataContainer.CustomMetaData, addFileBehaviour, targetIndexStreamStateBehaviour, targetDataStreamStateBehaviour);
        }

        public string GetStringData(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            var encoding = new ASCIIEncoding();
            return GetStringData(dataIdentifier, encoding, indexStreamStateBehaviour, dataStreamStateBehaviour);
        }

        public string GetStringData(Guid dataIdentifier, Encoding encoding, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            byte[] bytes = GetFileByteData(dataIdentifier, indexStreamStateBehaviour, dataStreamStateBehaviour);
            return encoding.GetString(bytes);
        }

        public IDynamiteXml GetObjectData(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            string serializedObjectString = GetStringData(dataIdentifier, indexStreamStateBehaviour, dataStreamStateBehaviour);
            return DynamiteXmlLogic.Deserialize(serializedObjectString);
        }

        public MetaDataContainer GetMetaDataContainer(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            MetaDataContainer result;

            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReading:
                    //
                    // open the stream and make a recursive call
                    //
                    using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, indexStreamStateBehaviour))
                    {
                        result = GetMetaDataContainer(dataIdentifier, StreamStateBehaviour.UseExistingStream, dataStreamStateBehaviour);
                    }
                    break;
                case StreamStateBehaviour.UseExistingStream:
                    // 
                    // check state of data file stream
                    //
                    switch (dataStreamStateBehaviour)
                    {
                        case StreamStateBehaviour.OpenNewStreamForReading:
                            //
                            // open the stream and make a recursive call
                            //
                            using (DataStream = FileStreamFactory.CreateFileStream(DataFilename, dataStreamStateBehaviour))
                            {
                                result = GetMetaDataContainer(dataIdentifier, indexStreamStateBehaviour, StreamStateBehaviour.UseExistingStream);
                            }
                            break;
                        case StreamStateBehaviour.UseExistingStream:
                            //
                            //
                            //
                            result = GetMetaDataContainerForExistingStreams(dataIdentifier);
                            //
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unexpected dataStreamStateBehaviour {0}", dataStreamStateBehaviour));
                    }
                    //
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }

            return result;
        }

        public MetaDataContainer GetMetaDataContainerForExistingStreams(Guid dataIdentifier)
        {
            MetaDataContainer result;

            if (!SupportsFeature(FileStorageFeatureEnum.StoreMetaData))
            {
                throw new NotSupportedException("Feature is not supported for this filestorage");
            }

            long offset = GetOffset(dataIdentifier, StreamStateBehaviour.UseExistingStream);

            if (offset != 0)
            {
                InterpretedDataFileDataStruct retrieveDataStruct = GetInterpretedDataStructUsingExistingStream(offset);

                //
                // double check the data is consistent
                //
                if (!retrieveDataStruct.DataIdentification.Equals(dataIdentifier))
                {
                    throw new Exception(string.Format("File allocation table is corrupt ({0},{1})", dataIdentifier, retrieveDataStruct.DataIdentification));
                }

                if (retrieveDataStruct.MetaDataOffsetInFileFromBegin == 0)
                {
                    // no meta data
                    result = null;
                }
                else
                {
                    DataStream.Seek(retrieveDataStruct.MetaDataOffsetInFileFromBegin, SeekOrigin.Begin);
                    var binaryReader = new BinaryReader(DataStream);
                    int metaDataLength = binaryReader.ReadInt32();
                    var metaDataBytes = new byte[metaDataLength];
                    var bufferedStream = new BufferedStream(DataStream);
                    bufferedStream.Read(metaDataBytes, 0, metaDataLength);
                    result = DynamiteXmlLogic.Deserialize<MetaDataContainer>(Encoding.UTF8.GetString(metaDataBytes));
                }
            }
            else
            {
                throw new Exception(string.Format("DataIdentifier {0} not found", dataIdentifier));
            }

            return result;
        }

        public byte[] GetFileByteData(Guid dataIdentifier, StreamStateBehaviour indexStreamStateBehaviour, StreamStateBehaviour dataStreamStateBehaviour)
        {
            byte[] result;

            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReading:
                    //
                    // open the stream and make a recursive call
                    //
                    using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, indexStreamStateBehaviour))
                    {
                        result = GetFileByteData(dataIdentifier, StreamStateBehaviour.UseExistingStream, dataStreamStateBehaviour);
                    }
                    break;
                case StreamStateBehaviour.UseExistingStream:
                    // 
                    // check state of data file stream
                    //
                    switch (dataStreamStateBehaviour)
                    {
                        case StreamStateBehaviour.OpenNewStreamForReading:
                            //
                            // open the stream and make a recursive call
                            //
                            using (DataStream = FileStreamFactory.CreateFileStream(DataFilename, dataStreamStateBehaviour))
                            {
                                result = GetFileByteData(dataIdentifier, indexStreamStateBehaviour, StreamStateBehaviour.UseExistingStream);
                            }
                            break;
                        case StreamStateBehaviour.UseExistingStream:
                            //
                            //
                            //
                            result = GetFileByteDataForExistingStreams(dataIdentifier);
                            //
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unexpected dataStreamStateBehaviour {0}", dataStreamStateBehaviour));
                    }
                    //
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }

            return result;
        }

        public byte[] GetFileByteDataForExistingStreams(Guid dataIdentifier)
        {
            byte[] result;

            long offset = GetOffset(dataIdentifier, StreamStateBehaviour.UseExistingStream);

            if (offset != 0)
            {
                InterpretedDataFileDataStruct retrieveFileStruct = GetInterpretedDataStructUsingExistingStream(offset);

                //
                // double check the data is consistent
                //
                if (!retrieveFileStruct.DataIdentification.Equals(dataIdentifier))
                {
                    throw new Exception(string.Format("File allocation table is corrupt ({0},{1})", dataIdentifier, retrieveFileStruct.DataIdentification));
                }

                DataStream.Seek(retrieveFileStruct.BinaryDataOffset, SeekOrigin.Begin);
                var binaryReader = new BinaryReader(DataStream);

                if (retrieveFileStruct.BinaryDataSizeInBytes > int.MaxValue)
                {
                    throw new Exception(string.Format("Data was stored ok, but cannot be retrieved in 1 chunk({0}})", dataIdentifier));
                }

                var numberOfBytesToRead = (int) retrieveFileStruct.BinaryDataSizeInBytes;

                result = binaryReader.ReadBytes(numberOfBytesToRead);
            }
            else
            {
                //
                // file does not exist (the offset is 0)
                //
                throw new DataIdentifierNotFoundException(string.Format("DataIdentifier {0} does not exist in filestorage {1}", dataIdentifier, FileStorageName));
            }

            return result;
        }

        public List<Guid> GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour indexStreamStateBehaviour)
        {
            return GetAllDataIdentifiersBasedUponFileStorageIndexFile(indexStreamStateBehaviour, null);
        }

        public List<Guid> GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour indexStreamStateBehaviour, ExposeProgressDelegate exposeProgressDelegate)
        {
            List<Guid> result;

            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.UseExistingStream:
                    result = new List<Guid>();
                    long offset = OffsetFirstIndexBlock;
                    var bytes = new List<byte>();
                    int depth = 0;

                    GetDataIdentifiersRecursiveUsingIndex(bytes, depth, offset, result, indexStreamStateBehaviour, exposeProgressDelegate);
                    break;

                case StreamStateBehaviour.OpenNewStreamForReading:
                    //
                    // open the index stream for reading and make a recursive call
                    //
                    using (IndexStream = new FileStream(IndexFilename, FileMode.Open, FileAccess.Read))
                    {
                        result = GetAllDataIdentifiersBasedUponFileStorageIndexFile(StreamStateBehaviour.UseExistingStream, exposeProgressDelegate);
                    }
                    break;

                default:
                    throw new NotSupportedException(string.Format("indexStreamStateBehaviour {0} is not supported", indexStreamStateBehaviour));
            }

            if (exposeProgressDelegate != null)
            {
                exposeProgressDelegate.Invoke(true);
            }

            return result;
        }

        private void GetDataIdentifiersRecursiveUsingIndex(IEnumerable<byte> bytesSoFar, int depth, Int64 offset, ICollection<Guid> result, StreamStateBehaviour indexStreamStateBehaviour, ExposeProgressDelegate anotherDataIdentifierFoundDelegate)
        {
            AllocationTableStruct indexStructs = GetIndexStruct(offset, indexStreamStateBehaviour);
            for (int currentByteValue = 0; currentByteValue <= 255; currentByteValue++)
            {
                if (indexStructs.Pointers[currentByteValue] == 0)
                {
                    // not used, recursion stops
                }
                else
                {
                    //
                    // clone bytes so far, extend with current byte
                    //
                    var bytesForGuid = new List<byte>();
                    bytesForGuid.AddRange(bytesSoFar);
                    bytesForGuid.Add((byte) currentByteValue);

                    if (depth == 15)
                    {
                        var newGuid = new Guid(bytesForGuid.ToArray());
                        result.Add(newGuid);

                        //
                        // notify outside world we found another hit, if the outside world wants to hear about it
                        //
                        if (anotherDataIdentifierFoundDelegate != null)
                        {
                            anotherDataIdentifierFoundDelegate.Invoke(newGuid);
                        }
                        //
                        // recursion stops; this is the last byte
                        //
                    }
                    else
                    {
                        //
                        // continue recursion
                        //
                        long newOffset = indexStructs.Pointers[currentByteValue];
                        GetDataIdentifiersRecursiveUsingIndex(bytesForGuid, depth + 1, newOffset, result, indexStreamStateBehaviour, anotherDataIdentifierFoundDelegate);
                    }
                }
            }
        }

        public IEnumerable GetAllDataIdentifierEnumerableBasedUponFileStorageDataFile()
        {
            //
            // FileStream dataStream
            //

            using (DataStream = new FileStream(DataFilename, FileMode.Open, FileAccess.Read))
            {
                long dataSize = DataStream.Length;
                long offset = OffsetFirstDataBlock;

                if (dataSize == offset)
                {
                    //
                    // data store is empty
                    //
                }
                else
                {
                    while (true)
                    {
                        //
                        // get offsets according to the data file
                        //
                        InterpretedDataFileDataStruct retrieveFileStruct = GetInterpretedDataStructUsingExistingStream(offset);
                        Guid dataIdentifierAccordingToDataFile = retrieveFileStruct.DataIdentification;
                        //
                        // the offset in the data file could be deprecated, for example when 
                        // the file (with a specific dataIdentifier) was update, or deleted
                        //
                        if (
                            GetOffset(dataIdentifierAccordingToDataFile, StreamStateBehaviour.OpenNewStreamForReading) ==
                            offset)
                        {
                            //
                            // item is still valid
                            //

                            //
                            // we yield the return; rather than first looping over the entire list and returning
                            // the end result, we immediately return the next item.
                            //
                            yield return retrieveFileStruct.DataIdentification;
                        }

                        //
                        // jump to the next data block
                        //
                        offset = retrieveFileStruct.BinaryDataOffset + retrieveFileStruct.BinaryDataSizeInBytes;

                        if (offset >= dataSize)
                        {
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Delete functionality

        /// <summary>
        ///     Marks the file in the index as deleted.
        ///     Note only the pointer to the file is removed, the data itself remains stored in the data file (data is not purged).
        /// </summary>
        public void DeleteDataIdentifier(Guid dataIdentifier, DeleteFileBehaviour deleteFileBehaviour, StreamStateBehaviour indexStreamStateBehaviour)
        {
            switch (indexStreamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReadingAndWriting:
                    //
                    // open the stream and make a recursive call
                    //
                    using (IndexStream = FileStreamFactory.CreateFileStream(IndexFilename, indexStreamStateBehaviour))
                    {
                        DeleteDataIdentifier(dataIdentifier, deleteFileBehaviour, StreamStateBehaviour.UseExistingStream);
                    }
                    break;
                case StreamStateBehaviour.UseExistingStream:
                    switch (deleteFileBehaviour)
                    {
                        case DeleteFileBehaviour.IgnoreWhenNotExists:
                            break;
                        case DeleteFileBehaviour.ThrowExceptionWhenNotExists:
                            if (!Exists(dataIdentifier, StreamStateBehaviour.UseExistingStream))
                            {
                                throw new FileNotFoundException(string.Format("File does not exist ({0})", dataIdentifier));
                            }
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unsupported delete file behaviour ({0})", deleteFileBehaviour));
                    }

                    DeleteDataIdentifierInExistingStream(dataIdentifier, 0, OffsetFirstIndexBlock, indexStreamStateBehaviour);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unexpected indexStreamStateBehaviour {0}", indexStreamStateBehaviour));
            }
        }

        private void DeleteDataIdentifierInExistingStream(Guid dataIdentifier, int currentIndexInGuid, Int64 currentOffset, StreamStateBehaviour indexStreamStateBehaviour)
        {
            byte[] bytes = dataIdentifier.ToByteArray();
            int indexerInGuid = bytes[currentIndexInGuid];

            AllocationTableStruct currentIndexStruct = GetIndexStruct(currentOffset, indexStreamStateBehaviour);

            if (currentIndexInGuid < 15)
            {
                long nextOffset = currentIndexStruct.Pointers[indexerInGuid];

                if (nextOffset == 0)
                {
                    throw new Exception("Unexpected; file was already deleted? (should have been checked before)");
                }

                //
                // recursively call BuildIndex for the next index in the guid
                //
                DeleteDataIdentifierInExistingStream(dataIdentifier, currentIndexInGuid + 1, nextOffset, indexStreamStateBehaviour);
            }
            else
            {
                switch (currentIndexInGuid)
                {
                    case 15:
                        if (currentIndexStruct.Pointers[indexerInGuid] == 0)
                        {
                            throw new Exception(
                                "Unexpected; file was already deleted? (should have been checked before)");
                        }
                        currentIndexStruct.Pointers[indexerInGuid] = 0;
                        StoreStruct(currentOffset, currentIndexStruct);
                        break;
                    default:
                        throw new IndexOutOfRangeException(currentIndexInGuid.ToString());
                }
            }
        }

        #endregion
    }
}