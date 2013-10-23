using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Model;

namespace MiSharp
{
    [Export(typeof (SettingsViewModel))]
    public class SettingsViewModel : Screen
    {
        private IOutputDevicePlugin _outSettingsViewModel;
        private List<int> _requestedLatency = new List<int>();

        public SettingsViewModel()
        {
            DisplayName = "Mi# Settings";
            RequestedLatency.AddRange(new[] {25, 50, 100, 150, 200, 300, 400, 500});
        }

        public string MediaPath
        {
            get { return Settings.Instance.WatchFolder; }
            set
            {
                Settings.Instance.WatchFolder = value;
                NotifyOfPropertyChange(() => MediaPath);
            }
        }

        public int RescanTimeout
        {
            get { return Settings.Instance.WatchFolderScanInterval; }
            set
            {
                Settings.Instance.WatchFolderScanInterval = value;
                NotifyOfPropertyChange(() => RescanTimeout);
            }
        }

        public string FileFormats
        {
            get { return string.Join(",", Settings.Instance.FileFormats); }
            set
            {
                Settings.Instance.FileFormats = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                NotifyOfPropertyChange(() => RescanTimeout);
            }
        }

        [ImportMany(typeof (IOutputDevicePlugin))]
        public IEnumerable<IOutputDevicePlugin> OutputDevicePlugins { get; set; }

        public IOutputDevicePlugin OutSettingsViewModel
        {
            get { return _outSettingsViewModel; }
            set
            {
                _outSettingsViewModel = value;
                NotifyOfPropertyChange(() => OutSettingsViewModel);
            }
        }

        public IOutputDevicePlugin SelectedOutputDriver
        {
            get { return Settings.Instance.SelectedOutputDriver; }
            set
            {
                //TODO: refactor switch
                switch (value.Name)
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
                //TODO:save concrete driver settings
                Settings.Instance.SelectedOutputDriver = value;
                NotifyOfPropertyChange(() => SelectedOutputDriver);
            }
        }

        public List<int> RequestedLatency
        {
            get { return _requestedLatency; }
            set
            {
                _requestedLatency = value;
                NotifyOfPropertyChange(() => RequestedLatency);
            }
        }

        public int SelectedLatency
        {
            get { return Settings.Instance.RequestedLatency; }
            set
            {
                Settings.Instance.RequestedLatency = value;
                NotifyOfPropertyChange(() => SelectedLatency);
            }
        }

        public IEnumerable<IResult> SaveClick()
        {
            Settings.Instance.SaveSettings();
            yield return new ChangesSaved();
        }
    }
}