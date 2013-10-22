using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using NAudio.Wave;

namespace MiSharp
{
    [Export(typeof (DirectSoundOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class DirectSoundOutSettingsViewModel : PropertyChangedBase, IOutputDevicePlugin
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
                _devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }

        public DirectSoundDeviceInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
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