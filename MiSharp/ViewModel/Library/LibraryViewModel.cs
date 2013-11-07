using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core.CustomEventArgs;

namespace MiSharp
{
    [Export]
    public class LibraryViewModel : Conductor<IScreen>.Collection.OneActive,
                                    IHandle<NavigationEventMessage>,
                                    IHandle<ScanCompletedEventArgs>
    {
        private IScreen _current;

        [ImportingConstructor]
        public LibraryViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            IoC.Get<INavigationService>().Navigate(typeof (ArtistNavigationViewModel), null);
        }

        public void Handle(NavigationEventMessage message)
        {
            _current = message.ViewModel;
            ActivateItem(message.ViewModel);
        }

        public void Handle(ScanCompletedEventArgs message)
        {
            _current.Refresh();
        }
    }
}