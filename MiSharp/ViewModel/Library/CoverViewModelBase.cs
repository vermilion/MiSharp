using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ReactiveUI;

namespace MiSharp
{
    public class CoverViewModelBase : ReactiveObject
    {
        private BitmapSource _cover;
        protected Func<BitmapSource> Func;

        protected CoverViewModelBase()
        {
            _cover = new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/MusicAndCatalog.ico"));
        }

        public BitmapSource Cover
        {
            get
            {
                Task.Run(() => Func())
                    .ContinueWith(task =>
                    {
                        this.RaiseAndSetIfChanged(ref _cover, task.Result);
                    });

                return _cover;
            }
        }
    }
}