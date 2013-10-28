using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using MiSharp.Core;
using MiSharp.Core.Library;
using MiSharp.Core.Player.Output;
using MiSharp.Core.Repository;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (SettingsViewModel))]
    public class SettingsViewModel : ReactiveScreen
    {
        private readonly IEventAggregator _events;
        private IOutputDevicePlugin _outSettingsViewModel;
        private List<int> _requestedLatency = new List<int>();

        [ImportingConstructor]
        public SettingsViewModel(IEventAggregator events)
        {
            DisplayName = "Mi# Settings";
            _events = events;
            RequestedLatency.AddRange(new[] {25, 50, 100, 150, 200, 300, 400, 500});
        }

        public string MediaPath
        {
            get { return Settings.Instance.WatchFolder; }
            set { this.RaiseAndSetIfChanged(ref Settings.Instance.WatchFolder, value); }
        }

        public int RescanTimeout
        {
            get { return Settings.Instance.WatchFolderScanInterval; }
            set { this.RaiseAndSetIfChanged(ref Settings.Instance.WatchFolderScanInterval, value); }
        }

        public string FileFormats
        {
            get { return string.Join(",", Settings.Instance.FileFormats); }
            set
            {
                this.RaiseAndSetIfChanged(ref Settings.Instance.FileFormats,
                                          value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        [ImportMany(typeof (IOutputDevicePlugin))]
        public IEnumerable<IOutputDevicePlugin> OutputDevicePlugins { get; set; }

        public IOutputDevicePlugin OutSettingsViewModel
        {
            get { return _outSettingsViewModel; }
            set { this.RaiseAndSetIfChanged(ref _outSettingsViewModel, value); }
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
                this.RaiseAndSetIfChanged(ref Settings.Instance.SelectedOutputDriver, value);
            }
        }

        public List<int> RequestedLatency
        {
            get { return _requestedLatency; }
            set { this.RaiseAndSetIfChanged(ref _requestedLatency, value); }
        }

        public int SelectedLatency
        {
            get { return Settings.Instance.RequestedLatency; }
            set { this.RaiseAndSetIfChanged(ref Settings.Instance.RequestedLatency, value); }
        }

        public IEnumerable<IResult> SaveClick()
        {
            Settings.Instance.SaveSettings();
            yield return new ChangesSaved();
        }

        public void RescanLibrary()
        {
            MediaRepository.Instance.Recreate();
            MediaRepository.Instance.ScanCompleted += Instance_ScanCompleted;
            //MediaRepository.Instance.FileFound += Instance_FileFound;
            Task.Run(() => MediaRepository.Instance.Rescan());
        }

        void Instance_ScanCompleted()
        {
            var model = IoC.Get<LibraryViewModel>();
            this.NotifyOfPropertyChange(() => model.Bands);
        }

        private void Instance_FileFound(FileStatEventArgs args)
        {
            _events.Publish(args);
        }
    }
}