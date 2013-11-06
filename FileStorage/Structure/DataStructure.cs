using System;

namespace FileStorage.Structure
{
    [Serializable]
    public struct DataFileHeaderStruct
    {
        public int FileStorageFeatures; // 4 bytes  (32 possible file features)
        public char[] Text; // 88 bytes, reserved / used for some informative data
        public int VersionMajor; // 4 bytes
        public int VersionMinor; // 4 bytes
    }

    [Serializable]
    public struct DataFileDataStruct
    {
        public Int64 BinaryDataSizeInBytes; // 8 bytes; the size in bytes
        public Guid DataIdentification; // 16 bytes; the unique identification of the guid 
        public int DataStateEnumID; // 4 bytes; state of the file, for example; active, deleted
        public Int64 MetaDataOffsetInFileFromBegin; // 8 bytes; pointer to meta data block
        public Int64 ReservedB; // 8 bytes; for future use
    }

    /// <summary>
    ///     Extends the FileStruct, by adding a file data offset, only used in memory (this is not persisted)
    /// </summary>
    public struct InterpretedDataFileDataStruct
    {
        public Int64 BinaryDataOffset;
        public Int64 BinaryDataSizeInBytes; //
        public Guid DataIdentification; //
        public int DataStateEnumID; // for example; active, deleted
        public Int64 MetaDataOffsetInFileFromBegin; // 8 bytes; pointer to meta data block
        public Int64 ReservedB; // for future use
    }
}