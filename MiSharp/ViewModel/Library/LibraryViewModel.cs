using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export]
    public class LibraryViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<NavigationEventMessage>
    {
        [ImportingConstructor]
        public LibraryViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            IoC.Get<INavigationService>().Navigate(typeof (ArtistNavigationViewModel), null);
        }

        public void Handle(NavigationEventMessage message)
        {
            ActivateItem(message.ViewModel);
        }
    }
}