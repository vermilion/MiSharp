using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export]
    public class SettingsAppearanceViewModel : PropertyChangedBase
    {
        public void ChangeAccentColor(string color)
        {
            //         Settings.Default.AccentColor =
        }
    }
}