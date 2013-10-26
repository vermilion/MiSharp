using System.ComponentModel.Composition;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class ShellViewModel : ReactiveScreen, IShellViewModel
    {
        private readonly IWindowManager _windowManager;
        private bool _isSettingsFlyoutOpen;

        [ImportingConstructor]
        public ShellViewModel(LibraryViewModel libraryViewModel, PlayerViewModel playerViewModel,
            SettingsBaseViewModel settingsBaseViewModel,
            PlaylistViewModel playlistViewModel,
            IWindowManager windowManager, IEventAggregator events)
        {
            DisplayName = "Mi#";
            LibraryViewModel = libraryViewModel;
            PlayerViewModel = playerViewModel;
            PlaylistViewModel = playlistViewModel;
            SettingsBaseViewModel = settingsBaseViewModel;
            events.Subscribe(this);
            _windowManager = windowManager;
        }

        public SettingsBaseViewModel SettingsBaseViewModel { get; set; }
        public LibraryViewModel LibraryViewModel { get; private set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public PlaylistViewModel PlaylistViewModel { get; set; }

        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isSettingsFlyoutOpen, value);
            }
        }

        public void OpenSettings()
        {
            IsSettingsFlyoutOpen = true;
        }

        public void SettingsClick()
        {
            var shell = IoC.Get<SettingsViewModel>();
            _windowManager.ShowDialog(shell);
        }
    }

    [InheritedExport]
    public interface IShellViewModel
    {
    }
}