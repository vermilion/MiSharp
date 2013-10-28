using System.ComponentModel.Composition;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using ReactiveUI;

namespace MiSharp
{
    [Export(typeof (ShellViewModel))]
    public class ShellViewModel : ReactiveScreen, IShellViewModel
    {
        private readonly IWindowManager _windowManager;
        private bool _isSettingsFlyoutOpen;

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator events)
        {
            DisplayName = "Mi#";

            LibraryViewModel = IoC.Get<LibraryViewModel>();
            PlayerViewModel = IoC.Get<PlayerViewModel>();
            PlaylistViewModel = IoC.Get<PlaylistViewModel>();
            SettingsBaseViewModel = IoC.Get<SettingsBaseViewModel>();
            events.Subscribe(this);
            _windowManager = IoC.Get<IWindowManager>();
        }

        public SettingsBaseViewModel SettingsBaseViewModel { get; set; }
        public LibraryViewModel LibraryViewModel { get; private set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public PlaylistViewModel PlaylistViewModel { get; set; }

        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref _isSettingsFlyoutOpen, value); }
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