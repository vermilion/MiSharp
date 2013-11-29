using Caliburn.Micro;
using MiSharp.ViewModel.Player;

namespace MiSharp.View.Player
{
    /// <summary>
    ///     Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView
    {
        public PlayerView()
        {
            InitializeComponent();

            SpectrumAnalyzer.RegisterSoundPlayer(IoC.Get<PlaybackController>());
        }
    }
}