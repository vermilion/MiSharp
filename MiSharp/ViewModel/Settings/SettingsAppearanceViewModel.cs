using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro;

namespace MiSharp
{
    [Export]
    public class SettingsAppearanceViewModel : PropertyChangedBase
    {
        private string _color = "Blue";
        private Theme _selectedTheme;

        public IEnumerable<Theme> Themes
        {
            get { return Enum.GetValues(typeof (Theme)).Cast<Theme>(); }
        }

        public Theme SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                ChangeAccentColor(_color);
            }
        }

        public void ChangeAccentColor(string color)
        {
            _color = color;
            ThemeManager.ChangeTheme(IoC.Get<ShellView>(),
                                     ThemeManager.DefaultAccents.First(accent => accent.Name == color), SelectedTheme);
        }
    }
}