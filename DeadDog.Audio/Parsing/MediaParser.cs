using System;
using System.Collections.Generic;
using DeadDog.Audio.Parsing.ID3;

namespace DeadDog.Audio.Parsing
{
    public class MediaParser : IDataParser
    {
        private readonly Dictionary<MediaTypes, IDataParser> _parsers = new Dictionary<MediaTypes, IDataParser>();

        public MediaParser()
        {
            foreach (MediaTypes type in Enum.GetValues(typeof (MediaTypes)))
                _parsers.Add(type, GetParser(type));
        }

        public MediaParser(MediaTypes types)
        {
            foreach (MediaTypes type in Enum.GetValues(typeof (MediaTypes)))
                if (types.HasFlag(type))
                    _parsers.Add(type, GetParser(type));
        }

        public RawTrack ParseTrack(string filepath)
        {
            int dot = filepath.LastIndexOf('.');
            if (dot == -1)
                throw new Exception("No file extension.");

            string ext = filepath.Substring(dot + 1);
            MediaTypes type = GetMediaType(ext);
            IDataParser parser = _parsers[type];

            return parser.ParseTrack(filepath);
        }

        private IDataParser GetParser(MediaTypes type)
        {
            switch (type)
            {
                case MediaTypes.Wma:
                    return null;
                case MediaTypes.Mp3:
                    return new ID3Parser();
                default:
                    throw new ArgumentException("Unknown media type.");
            }
        }

        private MediaTypes GetMediaType(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case "wma":
                    return MediaTypes.Wma;
                case "mp3":
                    return MediaTypes.Mp3;
                default:
                    throw new ArgumentException("Unknown media type.");
            }
        }
    }
}