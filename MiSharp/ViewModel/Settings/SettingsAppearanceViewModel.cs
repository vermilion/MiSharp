using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro;
using MiSharp.Core;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class SettingsAppearanceViewModel : ReactiveObject
    {
        public SettingsAppearanceViewModel()
        {
            ChangeAccentColor(Settings.Instance.AccentColor);
        }

        public IEnumerable<Theme> Themes
        {
            get { return Enum.GetValues(typeof (Theme)).Cast<Theme>(); }
        }

        public Theme SelectedTheme
        {
            get { return (Theme) Enum.Parse(typeof (Theme), Settings.Instance.SelectedTheme); }
            set
            {
                this.RaiseAndSetIfChanged(ref Settings.Instance.SelectedTheme, value.ToString());
                ChangeAccentColor(Settings.Instance.AccentColor);
            }
        }

        public void ChangeAccentColor(string color)
        {
            Settings.Instance.AccentColor = color;
            ThemeManager.ChangeTheme(IoC.Get<ShellView>(),
                                     ThemeManager.DefaultAccents.First(accent => accent.Name == color), SelectedTheme);
            Settings.Instance.SaveSettings();
        }
    }
}