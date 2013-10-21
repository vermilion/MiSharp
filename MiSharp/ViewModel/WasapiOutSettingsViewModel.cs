using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudioDemo.AudioPlaybackDemo;

namespace MiSharp
{
    [Export(typeof(WasapiOutSettingsViewModel))]
    public class WasapiOutSettingsViewModel : PropertyChangedBase, IOutputDevicePlugin
    {
        private List<MMDevice> _devices = new List<MMDevice>();

        public WasapiOutSettingsViewModel()
        {
            var enumerator = new MMDeviceEnumerator();
            var endPoints = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            
            foreach (var endPoint in endPoints)
            {
                Devices.Add(endPoint);
            }
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

        public MMDevice SelectedDevice { get; set; }
        public AudioClientShareMode ShareMode { get; set; }
        public bool UseEventCallback { get; set; }

        public IWavePlayer CreateDevice(int latency)
        {
            var wasapi = new WasapiOut(
                SelectedDevice,
                ShareMode,
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