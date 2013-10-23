using System.ComponentModel.Composition;
using NAudio.Wave;

namespace MiSharp.Model.Playlist.Input
{
    [Export(typeof (IInputFileFormatPlugin))]
    internal class Mp3InputFilePlugin : IInputFileFormatPlugin
    {
        public string Name
        {
            get { return "MP3 File"; }
        }

        public string Extension
        {
            get { return ".mp3"; }
        }

        public WaveStream CreateWaveStream(string fileName)
        {
            return new Mp3FileReader(fileName);
        }
    }
}