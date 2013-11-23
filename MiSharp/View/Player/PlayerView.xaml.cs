using Caliburn.Micro;

namespace MiSharp
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