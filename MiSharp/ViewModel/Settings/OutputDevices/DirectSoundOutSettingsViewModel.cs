using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MiSharp.Core.Player.Output;
using NAudio.Wave;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (DirectSoundOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class DirectSoundOutSettingsViewModel : ReactiveObject, IOutputDevicePlugin
    {
        private readonly bool _isAvailable;
        private List<DirectSoundDeviceInfo> _devices = new List<DirectSoundDeviceInfo>();
        private DirectSoundDeviceInfo _selectedDevice;


        public DirectSoundOutSettingsViewModel()
        {
            _isAvailable = DirectSoundOut.Devices.Any();
            Devices = DirectSoundOut.Devices.ToList();
            SelectedDevice = Devices.First();
        }

        public List<DirectSoundDeviceInfo> Devices
        {
            get { return _devices; }
            set
            {
                this.RaiseAndSetIfChanged(ref _devices, value);
            }
        }

        public DirectSoundDeviceInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDevice, value);
            }
        }

        public IWavePlayer CreateDevice(int latency)
        {
            return new DirectSoundOut(SelectedDevice.Guid, latency);
        }

        public string Name
        {
            get { return "DirectSound"; }
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
        }

        public int Priority
        {
            get { return 2; }
        }
    }
}