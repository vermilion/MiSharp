using System;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiSharp.Core;
using ReactiveUI;

namespace MiSharp.ViewModel.Settings
{
    [Export]
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;

        [ImportingConstructor]
        public SettingsViewModel(IEventAggregator events)
        {
            _events = events;

            Observable.Interval(TimeSpan.FromMinutes(1))
                      .Subscribe(x =>
                          {
                              TimeToNextRescan = TimeToNextRescan.Add(new TimeSpan(0, -1, 0));
                              if (TimeToNextRescan.TotalMinutes <= 0)
                              {
                                  RescanLibrary();
                                  TimeToNextRescan = TimeSpan.FromMinutes(RescanTimeout);
                              }
                          });
        }

        public string MediaPath
        {
            get { return Core.SettingsModel.Instance.WatchFolder; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.WatchFolder, value); }
        }

        public int RescanTimeout
        {
            get { return Core.SettingsModel.Instance.WatchFolderScanInterval; }
            set
            {
                this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.WatchFolderScanInterval, value);
                TimeToNextRescan = TimeSpan.FromMinutes(value);
            }
        }

        private TimeSpan TimeToNextRescan
        {
            get { return Core.SettingsModel.Instance.TimeToNextRescan; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.TimeToNextRescan, value); }
        }

        public string FileFormats
        {
            get { return string.Join(",", Core.SettingsModel.Instance.FileFormats); }
            set
            {
                this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.FileFormats,
                                          value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        public bool CoverDownloadEnabled
        {
            get { return Core.SettingsModel.Instance.CoverDownloadEnabled; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.CoverDownloadEnabled, value); }
        }

        public void RescanLibrary()
        {
            MediaScanner.Instance.ScanCompleted += e => _events.Publish(e);
            MediaScanner.Instance.FileFound += e => _events.Publish(e);
            Task.Run(() => MediaScanner.Instance.Rescan());
        }
    }
}