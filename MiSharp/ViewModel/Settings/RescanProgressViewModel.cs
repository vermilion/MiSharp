using System.ComponentModel.Composition;
using Caliburn.Micro;
using DeadDog.Audio.Scan;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class RescanProgressViewModel : ReactiveObject, IHandle<ScanFileEventArgs>
    {
        private string _textToShow;

        [ImportingConstructor]
        public RescanProgressViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
        }

        public string TextToShow
        {
            get { return _textToShow; }
            set { this.RaiseAndSetIfChanged(ref _textToShow, value); }
        }

        public void Handle(ScanFileEventArgs message)
        {
            TextToShow = string.Format("[{0}/{1}]: {2}",
                                       message.CurrentFileNumber,
                                       message.TotalFilesCount,
                                       message.Path);
        }
    }
}