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
            PlaylistManagerViewModel = IoC.Get<PlaylistManagerViewModel>();
            SettingsBaseViewModel = IoC.Get<SettingsBaseViewModel>();
            NowPlayingViewModel = IoC.Get<NowPlayingViewModel>();
            LibraryUpdateStateViewModel = IoC.Get<LibraryUpdateStateViewModel>();
            events.Subscribe(this);
            _windowManager = IoC.Get<IWindowManager>();
        }

        public SettingsBaseViewModel SettingsBaseViewModel { get; set; }
        public LibraryViewModel LibraryViewModel { get; private set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public PlaylistManagerViewModel PlaylistManagerViewModel { get; set; }
        public NowPlayingViewModel NowPlayingViewModel { get; set; }
        public LibraryUpdateStateViewModel LibraryUpdateStateViewModel { get; set; }
        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref _isSettingsFlyoutOpen, value); }
        }

        public void OpenSettings()
        {
            IsSettingsFlyoutOpen = true;
        }
    }

    [InheritedExport]
    public interface IShellViewModel
    {
    }
}