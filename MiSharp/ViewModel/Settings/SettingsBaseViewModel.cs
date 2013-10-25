using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export]
    public class SettingsBaseViewModel : Screen
    {
        [ImportingConstructor]
        public SettingsBaseViewModel(SettingsAppearanceViewModel settingsAppearanceViewModel, SettingsViewModel settingsViewModel)
        {
            SettingsAppearanceViewModel = settingsAppearanceViewModel;
            SettingsViewModel = settingsViewModel;
        }

        public SettingsAppearanceViewModel SettingsAppearanceViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }
    }
}