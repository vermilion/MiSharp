using NAudio.Wave;

namespace MiSharp.Core.Player.Output
{
    public interface IOutputDevicePlugin
    {
        string Name { get; }
        bool IsAvailable { get; }
        int Priority { get; }
        IWavePlayer CreateDevice(int latency);
    }
}