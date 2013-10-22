using NAudio.Wave;

namespace MiSharp
{
    public interface IOutputDevicePlugin
    {
        string Name { get; }
        bool IsAvailable { get; }
        int Priority { get; }
        IWavePlayer CreateDevice(int latency);
    }
}