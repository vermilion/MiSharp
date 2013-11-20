using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio.Scan;
using MiSharp.Core;

namespace MiSharp
{
    [Export]
    public class ShellViewModel : Screen, IHandle<ScanFileEventArgs>
    {
        private bool _isRescanFlyoutOpen;
        private bool _isSettingsFlyoutOpen;

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

        public void Handle(ScanFileEventArgs message)
        {
            IsRescanFlyoutOpen = message.TotalFilesCount != message.CurrentFileNumber;
        }

        public void OpenSettings()
        {
            IsSettingsFlyoutOpen = true;
        }

        // Post-closing events
        /// <summary>
        /// </summary>
        public void WindowClosed()
        {
            Settings.Instance.SaveSettings();
        }
    }
}