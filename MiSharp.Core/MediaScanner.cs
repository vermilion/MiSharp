using System.IO;
using System.Threading.Tasks;
using MiSharp.Core.CustomEventArgs;
using MiSharp.Core.Repository;

namespace MiSharp.Core
{
    public class MediaScanner
    {
        public delegate void FileFoundEventHandler(FileStatEventArgs e);

        public delegate void ScanCompletedEventHandler();

        private static MediaScanner _instance;

        private Library _lib;
        private AudioScanner _scanner;

        public static MediaScanner Instance
        {
            get { return _instance ?? (_instance = new MediaScanner()); }
        }

        public async Task<bool> Rescan()
        {
            _lib = MediaRepository.Instance.GetLibrary();
            _scanner = new AudioScanner(new ID3Parser(), Settings.Instance.WatchFolder, SearchOption.AllDirectories);
            _scanner.MediaLibrary = _lib;
            _scanner.ScanDone += scanner_ScanDone;
            _scanner.FileParsed += scanner_FileParsed;
            await Task.Run(() => _scanner.RunScannerAsync());

            return true;
        }


        public event FileFoundEventHandler FileFound;
        public event ScanCompletedEventHandler ScanCompleted;

        private void scanner_FileParsed(AudioScan sender, ScanFileEventArgs e)
        {
            if (FileFound != null)
                FileFound(new FileStatEventArgs());
        }

        private void scanner_ScanDone(AudioScan sender, DeadDog.Audio.Scan.ScanCompletedEventArgs e)
        {
            _lib = _scanner.MediaLibrary;
            MediaRepository.Instance.Save(_lib);
            ScanCompleted();
        }
    }
}