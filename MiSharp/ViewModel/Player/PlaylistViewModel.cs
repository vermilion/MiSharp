using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace MiSharp
{
    [Export(typeof (PlaylistViewModel))]
    public class PlaylistViewModel : Screen
    {
        [ImportingConstructor]
        public PlaylistViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
        }
    }
}