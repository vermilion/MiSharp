using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MiSharp.Core.Player.Output;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (WasapiOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class WasapiOutSettingsViewModel : ReactiveObject, IOutputDevicePlugin
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
            set { this.RaiseAndSetIfChanged(ref _devices, value); }
        }

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref _selectedDevice, value); }
        }

        //TODO: bind to AudioClientShareMode enum
        public bool ShareMode
        {
            get { return _shareMode; }
            set { this.RaiseAndSetIfChanged(ref _shareMode, value); }
        }

        public bool UseEventCallback
        {
            get { return _useEventCallback; }
            set { this.RaiseAndSetIfChanged(ref _useEventCallback, value); }
        }

        public IWavePlayer CreateDevice(int latency)
        {
            AudioClientShareMode shareMode = ShareMode
                ? AudioClientShareMode.Shared
                : AudioClientShareMode.Exclusive;
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