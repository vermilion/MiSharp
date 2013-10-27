using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using MiSharp.Core.Player.Output;
using NAudio.Wave;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (AsioOutSettingsViewModel))]
    [Export(typeof (IOutputDevicePlugin))]
    public class AsioOutSettingsViewModel : ReactiveObject, IOutputDevicePlugin
    {
        private List<string> _asioDrivers;

        public AsioOutSettingsViewModel()
        {
            AsioDrivers = AsioOut.GetDriverNames().ToList();
        }

        public List<string> AsioDrivers
        {
            get { return _asioDrivers; }
            set { this.RaiseAndSetIfChanged(ref _asioDrivers, value); }
        }

        public string SelectedDeviceName { get; set; }


        public IWavePlayer CreateDevice(int latency)
        {
            return new AsioOut(SelectedDeviceName);
        }

        public string Name
        {
            get { return "AsioOut"; }
        }

        public bool IsAvailable
        {
            get { return AsioOut.isSupported(); }
        }

        public int Priority
        {
            get { return 4; }
        }

        public void OpenControlPanel()
        {
            try
            {
                using (var asio = new AsioOut(SelectedDeviceName))
                {
                    asio.ShowControlPanel();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}