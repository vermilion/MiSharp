using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core.CustomEventArgs;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class LibraryUpdateStateViewModel : ReactiveObject, IHandle<ScanCompletedEventArgs>
    {
        private string _message;

        public LibraryUpdateStateViewModel()
        {
        }

        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }

        public bool IsOpen { get; set; }
        public void Handle(ScanCompletedEventArgs message)
        {
            Message = "Comleted";
        }
    }
}