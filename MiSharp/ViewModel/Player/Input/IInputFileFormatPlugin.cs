using NAudio.Wave;

namespace MiSharp
{
    public interface IInputFileFormatPlugin
    {
        string Name { get; }
        string Extension { get; }
        WaveStream CreateWaveStream(string fileName);
    }
}