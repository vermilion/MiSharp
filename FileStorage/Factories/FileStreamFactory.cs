using System;
using System.IO;
using FileStorage.Enums.Behaviours;

namespace FileStorage.Factories
{
    public static class FileStreamFactory
    {
        public static FileStream CreateFileStream(string filename, StreamStateBehaviour streamStateBehaviour)
        {
            FileStream result;

            switch (streamStateBehaviour)
            {
                case StreamStateBehaviour.OpenNewStreamForReading:
                    //
                    // we have to create a new one, and use recursion to get the actual return value
                    //
                    result = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    break;
                case StreamStateBehaviour.OpenNewStreamForReadingAndWriting:
                    //
                    // we have to create a new one, and use recursion to get the actual return value
                    //
                    result = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
                    break;
                default:
                    throw new NotSupportedException(string.Format("unknown streamStateBehaviour {0}", streamStateBehaviour));
            }

            return result;
        }
    }
}