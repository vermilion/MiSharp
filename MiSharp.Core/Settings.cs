using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiSharp.Core.Repository.Db4o;
using WPFSoundVisualizationLib;

namespace MiSharp.Core
{
    public class Settings
    {
        private static Settings _settings;

        public string[] FileFormats = new[] {"*.mp3"};

        public string WatchFolder = @"F:\_MUSIC";
        public int WatchFolderScanInterval = 60;
        public TimeSpan TimeToNextRescan;
        public string SelectedTheme = "Dark";
        public string AccentColor = "Blue";
        public bool RepeatState;
        public bool ShuffleState;
        public bool CoverDownloadEnabled = true;

        public bool EqualizerEnabled = false;
        public bool SoftBarFall = false;

        // Center: Frequency center. 20.0 to 22000.0. Default = 8000.0
        // Bandwidth: Octave range around the center frequency to filter. 0.2 to 5.0. Default = 1.0
        // Gain: Frequency Gain. 0.05 to 3.0. Default = 1.0
        public List<EqualizerParam> EqualizerValues = new List<EqualizerParam>
            {
                new EqualizerParam(32f, 1f, 1f),
                new EqualizerParam(64f, 1f, 1f),
                new EqualizerParam(125f, 1f, 1f),
                new EqualizerParam(250f, 1f, 1f),
                new EqualizerParam(500f, 1f, 1f),
                new EqualizerParam(1000f, 1f, 1f),
                new EqualizerParam(2000f, 1f, 1f),
                new EqualizerParam(4000f, 1f, 1f),
                new EqualizerParam(8000f, 1f, 1f),
                new EqualizerParam(16000f, 1f, 1f)
            };

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