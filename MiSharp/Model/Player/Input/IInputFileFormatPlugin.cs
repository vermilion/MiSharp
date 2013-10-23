using NAudio.Wave;

namespace MiSharp.Model.Playlist.Input
{
    public interface IInputFileFormatPlugin
    {
        string Name { get; }
        string Extension { get; }
        WaveStream CreateWaveStream(string fileName);
    }
}