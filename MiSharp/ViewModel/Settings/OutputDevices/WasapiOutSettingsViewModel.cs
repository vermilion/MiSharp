using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace MiSharp
{
    [Export(typeof (WasapiOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class WasapiOutSettingsViewModel : PropertyChangedBase, IOutputDevicePlugin
    {
        private List<MMDevice> _devices = new List<MMDevice>();
        private MMDevice _selectedDevice;
        private bool _shareMode;
        private bool _useEventCallback;

        public WasapiOutSettingsViewModel()
        {
            var enumerator = new MMDeviceEnumerator();
            MMDeviceCollection endPoints = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (MMDevice endPoint in endPoints)
            {
                Devices.Add(endPoint);
            }
            SelectedDevice = Devices.First();
        }

        public List<MMDevice> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
            }
        }

        //TODO: bind to AudioClientShareMode enum
        public bool ShareMode
        {
            get { return _shareMode; }
            set
            {
                _shareMode = value;
                NotifyOfPropertyChange(() => ShareMode);
            }
        }

        public bool UseEventCallback
        {
            get { return _useEventCallback; }
            set
            {
                _useEventCallback = value;
                NotifyOfPropertyChange(() => UseEventCallback);
            }
        }

        public IWavePlayer CreateDevice(int latency)
        {
            AudioClientShareMode shareMode = ShareMode ?
                                                 AudioClientShareMode.Shared :
                                                 AudioClientShareMode.Exclusive;
            var wasapi = new WasapiOut(
                SelectedDevice,
                shareMode,
                UseEventCallback,
                latency);
            return wasapi;
        }

        public string Name
        {
            get { return "WasapiOut"; }
        }

        public bool IsAvailable
        {
            // supported on Vista and above
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        public int Priority
        {
            get { return 3; }
        }
    }
}