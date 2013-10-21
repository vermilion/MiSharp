using NAudio.Wave;

namespace NAudioDemo.AudioPlaybackDemo
{
    public interface IInputFileFormatPlugin
    {
        string Name { get; }
        string Extension { get; }
        WaveStream CreateWaveStream(string fileName);
    }
}
