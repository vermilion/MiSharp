using System.Linq;
using System.Threading.Tasks;
using MiSharp.Core.Player.Output;
using MiSharp.Core.Repository.Db4o;

namespace MiSharp.Core
{
    public class Settings
    {
        private static Settings _settings;

        public string[] FileFormats;

        public int RequestedLatency;
        public IOutputDevicePlugin SelectedOutputDriver;
        public string WatchFolder;
        public int WatchFolderScanInterval;

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