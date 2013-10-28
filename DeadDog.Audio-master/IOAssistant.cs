using System;
using System.IO;
using System.Text;

namespace DeadDog.Audio
{
    /// <summary>
    ///     Provides simple IO methods for the DeadDog.Audio library. Capable of both reading and writing.
    /// </summary>
    internal class IOAssistant
    {
        private readonly Stream stream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IOAssistant" /> class from a specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to read/write.</param>
        public IOAssistant(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        ///     Reads a string (Unicode) using the specification defined by the Write method.
        /// </summary>
        /// <returns>The string read.</returns>
        public string ReadString()
        {
            return ReadString(Encoding.Unicode);
        }

        /// <summary>
        ///     Reads a string using the specification defined by the Write method.
        /// </summary>
        /// <param name="encoding">The encoding used for reading.</param>
        /// <returns>The string read.</returns>
        public string ReadString(Encoding encoding)
        {
            int len = ReadInt32();
            if (len == -1)
                return null;
            var buffer = new byte[len];
            stream.Read(buffer, 0, len);
            return encoding.GetString(buffer);
        }

        /// <summary>
        ///     Reads a 32bit signed integer from stream.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" /> read.
        /// </returns>
        public int ReadInt32()
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public void Write(string value)
        {
            Write(value, Encoding.Unicode);
        }

        public void Write(string value, Encoding encoding)
        {
            if (value == null)
            {
                Write(-1);
                return;
            }
            byte[] buffer = encoding.GetBytes(value);
            Write(buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Write(int value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }
    }
}