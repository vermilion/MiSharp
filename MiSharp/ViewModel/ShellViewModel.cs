using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio.Scan;

namespace MiSharp
{
    [Export(typeof (ShellViewModel))]
    public class ShellViewModel : Screen, IHandle<ScanFileEventArgs>
    {
        private bool _isSettingsFlyoutOpen;
        private bool _isRescanFlyoutOpen;

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator events)
        {
            DisplayName = "Mi#";

            LibraryViewModel = IoC.Get<LibraryViewModel>();
            PlayerViewModel = IoC.Get<PlayerViewModel>();
            SettingsBaseViewModel = IoC.Get<SettingsBaseViewModel>();
            NowPlayingViewModel = IoC.Get<NowPlayingViewModel>();
            RescanProgressViewModel = IoC.Get<RescanProgressViewModel>();

            events.Subscribe(this);
        }

        public SettingsBaseViewModel SettingsBaseViewModel { get; set; }
        public LibraryViewModel LibraryViewModel { get; private set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public NowPlayingViewModel NowPlayingViewModel { get; set; }
        public RescanProgressViewModel RescanProgressViewModel { get; set; }

        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set
            {
                _isSettingsFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsSettingsFlyoutOpen);
            }
        }

        public bool IsRescanFlyoutOpen
        {
            get { return _isRescanFlyoutOpen; }
            set
            {
                _isRescanFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsRescanFlyoutOpen);
            }
        }

        public void OpenSettings()
        {
            IsSettingsFlyoutOpen = true;
        }

        public void Handle(ScanFileEventArgs message)
        {
            IsRescanFlyoutOpen = message.TotalFilesCount != message.CurrentFileNumber;
        }
    }
}