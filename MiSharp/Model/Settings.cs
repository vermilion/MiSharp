using System.Linq;
using System.Threading.Tasks;
using MiSharp.Model.Repository;

namespace MiSharp.Model
{
    public class Settings
    {
        private static Settings _settings;
        private string _watchFolder;
        private int _watchFolderScanInterval;

        private Settings()
        {
            _watchFolderScanInterval = 60;
            _watchFolder = @"F:\_MUSIC";
        }

        public static Settings Instance
        {
            get { return _settings ?? (_settings = LoadSettings()); }
        }

        public int WatchFolderScanInterval
        {
            get { return _watchFolderScanInterval; }
            set
            {
                _watchFolderScanInterval = value;
                SaveSettings();
            }
        }

        public string WatchFolder
        {
            get { return _watchFolder; }
            set
            {
                _watchFolder = value;
                SaveSettings();
            }
        }

        private static Settings LoadSettings()
        {
            return SettingsRepository.Instance.GetAll<Settings>().FirstOrDefault() ?? new Settings();
        }

        public void SaveSettings()
        {
            Task.Run(() => SettingsRepository.Instance.Save(_settings));
        }
    }
}