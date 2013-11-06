using System;

namespace FileStorage.Structure
{
    [Serializable]
    public struct IndexFileHeaderStruct
    {
        public char[] Text; // 92 bytes, reserved / used for some informative data
        public int VersionMajor; // 4 bytes
        public int VersionMinor; // 4 bytes
    }

    [Serializable]
    public struct AllocationTableStruct
    {
        public Int64[] Pointers; // 256 pointers, for each byte of the guid
    }
}