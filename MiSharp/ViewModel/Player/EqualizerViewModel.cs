using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        public EqualizerViewModel()
        {
            DisplayName = "Equalizer";
        }
    }
}