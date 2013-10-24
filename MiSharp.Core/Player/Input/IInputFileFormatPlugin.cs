using NAudio.Wave;

namespace MiSharp.Core.Player.Input
{
    public interface IInputFileFormatPlugin
    {
        string Name { get; }
        string Extension { get; }
        WaveStream CreateWaveStream(string fileName);
    }
}