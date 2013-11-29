using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp.ViewModel.Player.Panes
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        public PlayerViewModel PlayerViewModel { get { return IoC.Get<PlayerViewModel>(); } }

        public EqualizerViewModel()
        {
            if (Core.SettingsModel.Instance.EqualizerEnabled)
                PlayerViewModel.PlaybackController.EqualizerEngine.InitEqualizer(Core.SettingsModel.Instance.EqualizerValues);
        }

        public bool EqualizerEnabled
        {
            get { return Core.SettingsModel.Instance.EqualizerEnabled; }
            set
            {
                Core.SettingsModel.Instance.EqualizerEnabled = value;
                if (value)
                {
                    PlayerViewModel.PlaybackController.EqualizerEngine.InitEqualizer(PlayerViewModel.PlaybackController.EqualizerEngine.BandsValues);
                }
                else
                {
                    PlayerViewModel.PlaybackController.EqualizerEngine.DeInitEqualizer();
                }
                NotifyOfPropertyChange(() => EqualizerEnabled);
            }
        }
    }
}