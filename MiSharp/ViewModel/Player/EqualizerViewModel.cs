using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.DialogResults;
using WPFSoundVisualizationLib;

namespace MiSharp
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        private readonly PlayerViewModel _playerViewModel;

        public EqualizerViewModel()
        {
            DisplayName = "Equalizer";
            _playerViewModel = IoC.Get<PlayerViewModel>();
            _playerViewModel.PlaybackController.EqualizerEngine.InitEqualizer(EqualizerData);
        }

        public List<EqualizerParam> EqualizerData
        {
            get { return Settings.Instance.EqualizerValues; }
            set
            {
                _playerViewModel.PlaybackController.EqualizerEngine.SetEqualizerValues(value);
                Settings.Instance.EqualizerValues = value;
                NotifyOfPropertyChange(() => EqualizerData);
            }
        }

        public bool EqualizerEnabled
        {
            get { return Settings.Instance.EqualizerEnabled; }
            set
            {
                Settings.Instance.EqualizerEnabled = value;
                if (value)
                {
                    _playerViewModel.PlaybackController.EqualizerEngine.InitEqualizer(EqualizerData);
                }
                else
                {
                    _playerViewModel.PlaybackController.EqualizerEngine.DeInitEqualizer();
                }
                NotifyOfPropertyChange(() => EqualizerEnabled);
            }
        }

        public IEnumerable<IResult> SaveChanges()
        {
            Settings.Instance.SaveSettings();
            yield return new CloseResult();
        }
    }
}