using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro;

namespace MiSharp
{
    [Export]
    public class SettingsAppearanceViewModel : PropertyChangedBase
    {
        public void ChangeAccentColor(string color)
        {
            ThemeManager.ChangeTheme(IoC.Get<ShellView>(), ThemeManager.DefaultAccents.First(accent => accent.Name == color), Theme.Light);
        }
    }
}