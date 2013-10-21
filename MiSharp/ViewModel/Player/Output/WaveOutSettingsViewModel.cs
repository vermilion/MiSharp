using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NAudio.Wave;

namespace MiSharp
{
    [Export(typeof (WaveOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class WaveOutSettingsViewModel : PropertyChangedBase, IOutputDevicePlugin
    {
        private List<WaveCallbackStrategy> _callBacks = new List<WaveCallbackStrategy>();
        private List<string> _devices = new List<string>();

        public WaveOutSettingsViewModel()
        {
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(deviceId);
                Devices.Add(String.Format("Device {0} ({1})", deviceId, capabilities.ProductName));
            }

            CallBacks.AddRange(new List<WaveCallbackStrategy>
            {
                WaveCallbackStrategy.NewWindow,
                WaveCallbackStrategy.FunctionCallback,
                WaveCallbackStrategy.Event
            });
        }

        public List<string> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }

        public List<WaveCallbackStrategy> CallBacks
        {
            get { return _callBacks; }
            set
            {
                _callBacks = value;
                NotifyOfPropertyChange(() => CallBacks);
            }
        }

        public int SelectedDeviceNumber { get; set; }
        public WaveCallbackStrategy CallbackStrategy { get; set; }


        public IWavePlayer CreateDevice(int latency)
        {
            IWavePlayer device = new WaveOut();
            WaveCallbackStrategy strategy = CallbackStrategy;
            if (strategy == WaveCallbackStrategy.Event)
            {
                var waveOut = new WaveOutEvent();
                //   waveOut.DeviceNumber = waveOutSettingsPanel.SelectedDeviceNumber;
                waveOut.DesiredLatency = latency;
                device = waveOut;
            }
            else
            {
                WaveCallbackInfo callbackInfo = strategy == WaveCallbackStrategy.NewWindow
                    ? WaveCallbackInfo.NewWindow()
                    : WaveCallbackInfo.FunctionCallback();
                var outputDevice = new WaveOut(callbackInfo);
                outputDevice.DeviceNumber = SelectedDeviceNumber;
                outputDevice.DesiredLatency = latency;
                device = outputDevice;
            }
            // TODO: configurable number of buffers

            return device;
        }

        public string Name
        {
            get { return "WaveOut"; }
        }

        public bool IsAvailable
        {
            get { return WaveOut.DeviceCount > 0; }
        }

        public int Priority
        {
            get { return 1; }
        }
    }
}