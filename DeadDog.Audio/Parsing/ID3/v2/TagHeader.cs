using System;
using System.IO;
using System.Text;

namespace DeadDog.Audio.Parsing.ID3
{
    public class TagHeader
    {
        private static readonly TagHeader empty;
        private readonly long firstframe;

        private readonly TagFlags flags;
        private readonly int size;
        private readonly Version version;
        private bool isempty;

        static TagHeader()
        {
            empty = new TagHeader(new Version(), 0, 0, 0);
            empty.isempty = true;
        }

        public TagHeader(FrameReader reader)
        {
            var buffer = new byte[9];
            bool found = false;
            int offset = 0;
            version = new Version(0, 0);
            while (reader.stream.Position < reader.stream.Length && !found && reader.stream.Position < 200)
            {
                int b = reader.stream.ReadByte();
                if (b == 0x49)
                {
                    reader.stream.Read(buffer, offset, 9);
                    string text = Encoding.ASCII.GetString(buffer);
                    if (buffer[0] == 0x44 &&
                        buffer[1] == 0x33 &&
                        buffer[2] < 0xff &&
                        buffer[3] < 0xff &&
                        //buffer[4] is the flag byte
                        buffer[5] < 0x80 &&
                        buffer[6] < 0x80 &&
                        buffer[7] < 0x80 &&
                        buffer[8] < 0x80)
                    {
                        version = new Version(buffer[2], buffer[3]);
                        flags = (TagFlags) buffer[4];
                        size = BinaryConverter.ToInt32(buffer, 5, true);

                        if (size + reader.stream.Position > reader.stream.Length)
                        {
                            version = new Version(0, 0);
                            break;
                        }

                        if ((flags & TagFlags.ExtendedHeader) == TagFlags.ExtendedHeader)
                        {
                            reader.stream.Read(buffer, 0, 4);
                            int extendedsize = BinaryConverter.ToInt32(buffer, 0, true);
                            reader.stream.Seek(extendedsize - 4, SeekOrigin.Current);
                        }

                        firstframe = reader.stream.Position;
                        break;
                    }
                    else
                        reader.stream.Seek(-9, SeekOrigin.Current);
                }
            }
            if (version == new Version(0, 0))
            {
                isempty = true;
                version = empty.version;
                flags = empty.flags;
                size = empty.size;
                firstframe = empty.firstframe;
            }
        }

        public TagHeader(Version version, TagFlags flags, int size, long firstframe)
        {
            this.version = version;
            this.flags = flags;
            this.size = size;
            this.firstframe = firstframe;
        }

        public static TagHeader Empty
        {
            get { return empty; }
        }

        public Version Version
        {
            get { return version; }
        }

        public TagFlags Flags
        {
            get { return flags; }
        }

        public int Size
        {
            get { return size; }
        }

        public long FirstFramePosition
        {
            get { return firstframe; }
        }

        public static bool IsEmpty(TagHeader item)
        {
            return item.isempty;
        }

        public static bool operator ==(TagHeader left, TagHeader right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            if (ReferenceEquals(right, null))
                return false;
            if (left.isempty && right.isempty)
                return true;
            else
                return ReferenceEquals(left, right);
        }

        public static bool operator !=(TagHeader left, TagHeader right)
        {
            if (left.isempty && right.isempty)
                return false;
            else
                return !ReferenceEquals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TagHeader))
                return false;
            var o = obj as TagHeader;
            return o.isempty == isempty &&
                   o.version == version &&
                   o.flags == flags &&
                   o.size == size &&
                   o.firstframe == firstframe;
        }

        public override int GetHashCode()
        {
            if (isempty)
                return -1;
            return version.GetHashCode();
        }
    }
}