using System.ComponentModel.Composition;
using NAudio.Wave;

namespace MiSharp
{
    [Export(typeof (IInputFileFormatPlugin))]
    internal class AiffInputFilePlugin : IInputFileFormatPlugin
    {
        public string Name
        {
            get { return "AIFF File"; }
        }

        public string Extension
        {
            get { return ".aiff"; }
        }

        public WaveStream CreateWaveStream(string fileName)
        {
            return new AiffFileReader(fileName);
        }
    }
}