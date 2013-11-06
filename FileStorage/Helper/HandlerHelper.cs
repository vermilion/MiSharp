using System;
using System.IO;

namespace FileStorage.Helper
{
    /// <summary>
    ///     Taken from
    ///     http://geekswithblogs.net/sdorman/archive/2009/01/10/reading-all-bytes-from-a-stream.aspx
    /// </summary>
    public static class HandlerHelper
    {
        public static byte[] ReadAllBytes(this Stream source)
        {
            var readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = source.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = source.ReadByte();
                    if (nextByte != -1)
                    {
                        var temp = new byte[readBuffer.Length*2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
    }
}