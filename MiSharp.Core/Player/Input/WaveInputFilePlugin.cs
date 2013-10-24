using System.ComponentModel.Composition;
using NAudio.Wave;

namespace MiSharp.Core.Player.Input
{
    [Export(typeof (IInputFileFormatPlugin))]
    internal class WaveInputFilePlugin : IInputFileFormatPlugin
    {
        public string Name
        {
            get { return "WAV file"; }
        }

        public string Extension
        {
            get { return ".wav"; }
        }

        public WaveStream CreateWaveStream(string fileName)
        {
            WaveStream readerStream = new WaveFileReader(fileName);
            if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm &&
                readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                readerStream = new BlockAlignReductionStream(readerStream);
            }
            return readerStream;
        }
    }
}