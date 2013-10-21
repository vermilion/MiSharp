using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NAudioDemo.AudioPlaybackDemo;

namespace MiSharp
{
    [Export(typeof (PlayerViewModel))]
    public class PlayerViewModel : Screen
    {
        public List<IOutputDevicePlugin> OutputDevicePlugins { get; set; }

        public IOutputDevicePlugin OutSettingsViewModel
        {
            get { return _outSettingsViewModel; }
            set
            {
                _outSettingsViewModel = value;
                NotifyOfPropertyChange(() => OutSettingsViewModel);
            }
        }

        public PlayerViewModel()
        {
            OutputDevicePlugins = new List<IOutputDevicePlugin>()
                {
                    new AsioOutSettingsViewModel(),
                    new WasapiOutSettingsViewModel(),
                    new WaveOutSettingsViewModel(),
                    new DirectSoundOutSettingsViewModel()
                };
        }

        private IOutputDevicePlugin _selectedOutPutDriver;
        private IOutputDevicePlugin _outSettingsViewModel;

        public IOutputDevicePlugin SelectedOutPutDriver
        {
            get { return _selectedOutPutDriver; }
            set
            {
                _selectedOutPutDriver = value;
                switch (_selectedOutPutDriver.Name)
                {
                    case "AsioOut":
                        OutSettingsViewModel = new AsioOutSettingsViewModel();
                        break;
                    case "WaveOut":
                        OutSettingsViewModel = new WaveOutSettingsViewModel();
                        break;
                    case "WasapiOut":
                        OutSettingsViewModel = new WasapiOutSettingsViewModel();
                        break;
                    case "DirectSound":
                        OutSettingsViewModel = new DirectSoundOutSettingsViewModel();
                        break;
                }
                NotifyOfPropertyChange(() => SelectedOutPutDriver);
            }
        }
    }
}