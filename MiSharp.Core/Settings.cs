using System.Linq;
using System.Threading.Tasks;
using MiSharp.Core.Player.Output;
using MiSharp.Core.Repository;

namespace MiSharp.Core
{
    public class Settings
    {
        private static Settings _settings;

        private Settings()
        {
            WatchFolderScanInterval = 60;
            WatchFolder = @"F:\_MUSIC";
            FileFormats = new[] {"*.mp3"};
        }

        public static Settings Instance
        {
            get { return _settings ?? (_settings = LoadSettings()); }
        }

        public int WatchFolderScanInterval;

        public string WatchFolder;

        public string[] FileFormats;

        public IOutputDevicePlugin SelectedOutputDriver;

        public int RequestedLatency;

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