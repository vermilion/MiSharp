using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.DialogResults;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IEventAggregator _events;
        private List<int> _requestedLatency = new List<int>();

        [ImportingConstructor]
        public SettingsViewModel(IEventAggregator events)
        {
            _events = events;
            RequestedLatency.AddRange(new[] {25, 50, 100, 150, 200, 300, 400, 500});

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
            get { return Settings.Instance.WatchFolder; }
            set { this.RaiseAndSetIfChanged(ref Settings.Instance.WatchFolder, value); }
        }

        public int RescanTimeout
        {
            get { return Settings.Instance.WatchFolderScanInterval; }
            set
            {
                this.RaiseAndSetIfChanged(ref Settings.Instance.WatchFolderScanInterval, value);
                TimeToNextRescan = TimeSpan.FromMinutes(value);
            }
        }

        private TimeSpan TimeToNextRescan
        {
            get { return Settings.Instance.TimeToNextRescan; }
            set { this.RaiseAndSetIfChanged(ref Settings.Instance.TimeToNextRescan, value); }
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
            yield return new DummyResult();
        }

        public void RescanLibrary()
        {
            MediaScanner.Instance.ScanCompleted += e => _events.Publish(e);
            MediaScanner.Instance.FileFound += e => _events.Publish(e);
            Task.Run(() => MediaScanner.Instance.Rescan());
        }
    }
}