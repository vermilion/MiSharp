using System;
using System.Collections.Generic;
using System.IO;

namespace DeadDog.Audio.Parsing.ID3
{
    public class FrameReader : IDisposable
    {
        private readonly List<FrameInfo> frames = new List<FrameInfo>();

        private readonly TagHeader header;
        internal List<Exception> parsingerrors = new List<Exception>();
        internal Stream stream;

        public FrameReader(Stream input)
        {
            stream = input;
            stream.Seek(0, SeekOrigin.Begin);

            header = new TagHeader(this);
            if (header == null || header.Size <= 0)
            {
                parsingerrors.Add(new Exception("Unable to parse."));
                return;
            }

            if (header.Version.Major == 2)
            {
                parsingerrors.Add(new Exception("ID3 v2.2 not supported"));
            }
            else if (header.Version.Major > 2)
            {
                while (stream.Position < header.FirstFramePosition + header.Size)
                {
                    BinaryConverter.Skip(0, stream, header.FirstFramePosition + header.Size);
                    if (stream.Position >= header.FirstFramePosition + header.Size)
                        break;

                    FrameInfo frame;
                    try
                    {
                        frame = new FrameInfo(stream, header, true);
                    }
                    catch (Exception e)
                    {
                        parsingerrors.Add(e);
                        frame = null;
                        stream.Seek(0, SeekOrigin.End);
                    }

                    if (frame != null)
                    {
                        frames.Add(frame);
                    }
                }
            }
            else
            {
                parsingerrors.Add(new Exception("Unsupported ID3-version"));
            }
        }

        public int FrameCount
        {
            get { return frames.Count; }
        }

        public TagHeader TagHeader
        {
            get { return header; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            stream = null;
        }

        #endregion

        public T Read<T>(string identifier, Reader<T> reader)
        {
            return Read(identifier, reader, default(T));
        }

        public T Read<T>(string identifier, Reader<T> reader, T notfound)
        {
            FrameInfo frame = findFrame(identifier);
            return Read(frame, reader, notfound);
        }

        private T Read<T>(FrameInfo frame, Reader<T> reader, T notfound)
        {
            if (frame == null)
                return notfound;
            stream.Seek(frame.Position, SeekOrigin.Begin);
            var buffer = new byte[frame.Size];
            stream.Read(buffer, 0, frame.Size);

            T result;

            using (var ms = new MemoryStream(buffer))
            using (var dr = new BinaryReader(ms))
            {
                try
                {
                    result = reader(dr);
                }
                catch
                {
                    result = notfound;
                }
            }
            return result;
        }

        private FrameInfo findFrame(string identifier)
        {
            if (identifier.Length != 4)
                throw new ArgumentException("A frame identifier must be exactly 4 characters long.", "identifier");
            for (int i = 0; i < frames.Count; i++)
                if (frames[i].Identifier == identifier)
                    return frames[i];
            return null;
        }

        public bool Contains(string identifier)
        {
            if (identifier.Length != 4)
                throw new ArgumentException("A frame identifier must be exactly 4 characters long.", "identifier");
            for (int i = 0; i < frames.Count; i++)
                if (frames[i].Identifier == identifier)
                    return true;
            return false;
        }

        public string[] GetIdentifiers()
        {
            var ids = new string[frames.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = frames[i].Identifier;
            return ids;
        }
    }

    public delegate T Reader<T>(BinaryReader reader);
}