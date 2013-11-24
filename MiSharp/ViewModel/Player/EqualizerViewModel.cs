using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;


namespace MiSharp
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        public PlayerViewModel PlayerViewModel { get { return IoC.Get<PlayerViewModel>(); } }

        public bool EqualizerEnabled
        {
            get { return Settings.Instance.EqualizerEnabled; }
            set
            {
                Settings.Instance.EqualizerEnabled = value;
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