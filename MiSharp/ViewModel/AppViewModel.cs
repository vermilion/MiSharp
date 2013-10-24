using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export(typeof (AppViewModel))]
    public class AppViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public AppViewModel(LibraryViewModel libraryViewModel, PlayerViewModel playerViewModel,
                            PlaylistViewModel playlistViewModel,
                            IWindowManager windowManager, IEventAggregator events)
        {
            DisplayName = "Mi#";
            LibraryViewModel = libraryViewModel;
            PlayerViewModel = playerViewModel;
            PlaylistViewModel = playlistViewModel;
            events.Subscribe(this);
            _windowManager = windowManager;
        }

        public LibraryViewModel LibraryViewModel { get; private set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public PlaylistViewModel PlaylistViewModel { get; set; }

        public void SettingsClick()
        {
            var shell = IoC.Get<SettingsViewModel>();
            _windowManager.ShowDialog(shell);
        }
    }
}