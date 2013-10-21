using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using NAudio.Wave;
using NAudioDemo.AudioPlaybackDemo;
using MessageBox = System.Windows.MessageBox;

namespace MiSharp
{
    [Export(typeof(AsioOutSettingsViewModel))]
    public class AsioOutSettingsViewModel : PropertyChangedBase, IOutputDevicePlugin
    {
        private List<string> _asioDrivers;

        public AsioOutSettingsViewModel()
        {
            AsioDrivers = AsioOut.GetDriverNames().ToList();
        }

        public List<string> AsioDrivers
        {
            get { return _asioDrivers; }
            set
            {
                _asioDrivers = value;
                NotifyOfPropertyChange(() => AsioDrivers);
            }
        }

        public string SelectedDeviceName { get; set; }

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
    }
}