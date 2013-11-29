using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro;
using MiSharp.View;
using ReactiveUI;

namespace MiSharp.ViewModel.Settings
{
    [Export]
    public class SettingsAppearanceViewModel : ReactiveObject
    {
        public SettingsAppearanceViewModel()
        {
            ChangeAccentColor(Core.SettingsModel.Instance.AccentColor);
        }

        public IEnumerable<Theme> Themes
        {
            get { return Enum.GetValues(typeof (Theme)).Cast<Theme>(); }
        }

        public Theme SelectedTheme
        {
            get { return (Theme) Enum.Parse(typeof (Theme), Core.SettingsModel.Instance.SelectedTheme); }
            set
            {
                this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.SelectedTheme, value.ToString());
                ChangeAccentColor(Core.SettingsModel.Instance.AccentColor);
            }
        }

        public void ChangeAccentColor(string color)
        {
            Core.SettingsModel.Instance.AccentColor = color;
            ThemeManager.ChangeTheme(IoC.Get<ShellView>(),
                                     ThemeManager.DefaultAccents.First(accent => accent.Name == color), SelectedTheme);
        }

        public bool SoftBarFall
        {
            get { return Core.SettingsModel.Instance.SoftBarFall; }
            set { this.RaiseAndSetIfChanged(ref Core.SettingsModel.Instance.SoftBarFall, value); }
        }
    }
}