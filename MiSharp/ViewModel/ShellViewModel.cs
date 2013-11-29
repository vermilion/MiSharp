using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using DeadDog.Audio.Scan;
using MiSharp.Core;
using MiSharp.ViewModel.Library;
using MiSharp.ViewModel.Player;
using MiSharp.ViewModel.Settings;

namespace MiSharp.ViewModel
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
            events.Subscribe(this);
        }

        public SettingsBaseViewModel SettingsBaseViewModel
        {
            get { return IoC.Get<SettingsBaseViewModel>(); }
        }

        public LibraryViewModel LibraryViewModel
        {
            get { return IoC.Get<LibraryViewModel>(); }
        }

        public PlayerViewModel PlayerViewModel
        {
            get { return IoC.Get<PlayerViewModel>(); }
        }

        public RescanProgressViewModel RescanProgressViewModel
        {
            get { return IoC.Get<RescanProgressViewModel>(); }
        }

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

        private double _prevWidth;
        private bool _isMiniWindow;

        public void SwitchMode()
        {
            var mainWindow = Application.Current.Windows.OfType<Window>().First();
            if (_isMiniWindow)
            {
                mainWindow.Width = _prevWidth;
                _isMiniWindow = false;
            }
            else
            {
                _prevWidth = mainWindow.Width;
                mainWindow.Width = 450;
                _isMiniWindow = true;
            }
        }

        public void KeyDownClick(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                PlayerViewModel.PlayPauseCommand.Execute(null);
        }

        // Post-closing events
        /// <summary>
        /// </summary>
        public void WindowClosed()
        {
            SettingsModel.Instance.EqualizerValues = PlayerViewModel.PlaybackController.EqualizerEngine.BandsValues;
            IoC.Get<PlaybackController>().Dispose();

            SettingsModel.Instance.SaveSettings();
        }
    }
}