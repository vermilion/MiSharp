using System.Linq;
using System.Threading.Tasks;
using MiSharp.Model.Repository;

namespace MiSharp.Model
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

        public int WatchFolderScanInterval { get; set; }

        public string WatchFolder { get; set; }

        public string[] FileFormats { get; set; }

        public IOutputDevicePlugin SelectedOutputDriver { get; set; }

        public int RequestedLatency { get; set; }

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