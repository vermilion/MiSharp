using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Model;

namespace MiSharp
{
    [Export(typeof (SettingsViewModel))]
    public class SettingsViewModel : Screen
    {
        public SettingsViewModel()
        {
            DisplayName = "Mi# Settings";
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
    }
}