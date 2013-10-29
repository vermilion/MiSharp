using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DeadDog.Audio.Parsing.ID3
{
    public class ID3v2
    {
        private string album;
        private string artist;
        private int frameCount;
        private TagHeader header;
        private string title;
        private int tracknumber = -1;
        private string trackstring;
        private int year = -1;
        private string yearstring;

        public ID3v2(Stream stream)
        {
            using (var reader = new FrameReader(stream))
            {
                setvalues(reader);
            }
        }

        public ID3v2(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open))
            using (var reader = new FrameReader(fs))
            {
                setvalues(reader);
            }
        }

        public bool TagFound
        {
            get { return !TagHeader.IsEmpty(header) && frameCount > 0; }
        }

        public string Artist
        {
            get { return artist; }
        }

        public string Album
        {
            get { return album; }
        }

        public string Title
        {
            get { return title; }
        }

        public string TrackString
        {
            get { return trackstring; }
        }

        public int TrackNumber
        {
            get { return tracknumber; }
        }

        public string YearString
        {
            get { return yearstring; }
        }

        public int Year
        {
            get { return year; }
        }

        private void setvalues(FrameReader reader)
        {
            frameCount = reader.FrameCount;
            header = reader.TagHeader;
            title = reader.Read("TIT2", ReadString, null);
            artist = reader.Read("TPE1", ReadString, null);
            album = reader.Read("TALB", ReadString, null);
            yearstring = reader.Read("TYER", ReadString, null);
            if (yearstring == null)
                year = -1;
            else
            {
                Match regex = Regex.Match(yearstring, "[0-9]{4,4}");
                if (regex.Success)
                    year = int.Parse(regex.Value);
                else
                    year = -1;
            }
            trackstring = reader.Read("TRCK", ReadString, null);

            if (trackstring == null)
            {
                tracknumber = -1;
            }
            else if (trackstring.Contains("/"))
            {
                string s = trackstring.Substring(0, trackstring.IndexOf('/'));

                tracknumber = -1;
                int.TryParse(s, out tracknumber);
            }
            else
            {
                var sb = new StringBuilder();
                for (int i = 0; i < trackstring.Length; i++)
                {
                    if (char.IsDigit(trackstring[i]))
                        sb.Append(trackstring[i]);
                }
                tracknumber = -1;
                int.TryParse(sb.ToString(), out tracknumber);
            }
        }

        public static string ReadString(BinaryReader reader)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding enc = null;
            byte[] data = reader.ReadBytes((int) reader.BaseStream.Length);
            if (data.Length <= 1)
                return null;
            string s;
            switch (data[0])
            {
                case 0x00:
                    enc = iso;
                    s = enc.GetString(data, 1, data.Length - 1);
                    break;
                case 0x01:
                    if (data[1] == 0xFF && data[2] == 0xFE)
                        enc = Encoding.Unicode;
                    else if (data[1] == 0xFE && data[2] == 0xFF)
                        enc = Encoding.BigEndianUnicode;
                    else
                        throw new Exception("Unknown Encoding");

                    s = enc.GetString(data, 3, data.Length - 3);
                    ;
                    break;
                case 0x02:
                    enc = Encoding.BigEndianUnicode;
                    s = enc.GetString(data, 1, data.Length - 1);
                    break;
                case 0x03:
                    enc = Encoding.UTF8;
                    s = enc.GetString(data, 1, data.Length - 1);
                    break;
                default:
                    throw new Exception("Unknown Encoding");
            }
            s = s.Trim('\0');
            return s;
        }
    }
}