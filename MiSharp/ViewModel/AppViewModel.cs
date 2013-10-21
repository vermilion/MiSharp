using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export(typeof (AppViewModel))]
    public class AppViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public AppViewModel(LibraryViewModel libraryViewModel, IWindowManager windowManager, IEventAggregator events)
        {
            DisplayName = "Mi#";
            LibraryViewModel = libraryViewModel;
            events.Subscribe(this);
            _windowManager = windowManager;
        }

        public LibraryViewModel LibraryViewModel { get; private set; }

        //public void Handle(ColorEvent message)
        //{
        //    Color = message.Color;
        //}

        public void SettingsClick()
        {
            _windowManager.ShowDialog(new SettingsViewModel());
        }

        public void PlayerClick()
        {
            _windowManager.ShowDialog(new PlayerViewModel());
        }
    }
}