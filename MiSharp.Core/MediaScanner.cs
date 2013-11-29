using System.IO;
using System.Threading.Tasks;
using DeadDog.Audio.Libraries;
using DeadDog.Audio.Parsing.Parsers;
using DeadDog.Audio.Scan;
using DeadDog.Audio.Scan.AudioScanner;
using MiSharp.Core.Repository.Db4o;

namespace MiSharp.Core
{
    public class MediaScanner
    {
        public delegate void FileFoundEventHandler(ScanFileEventArgs e);

        public delegate void ScanCompletedEventHandler(ScanCompletedEventArgs e);

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
            _scanner = new AudioScanner(new ID3Parser(), SettingsModel.Instance.WatchFolder, SearchOption.AllDirectories);
            _scanner.MediaLibrary = _lib;
            _scanner.ScanDone += ScannerScanDone;
            _scanner.FileParsed += ScannerFileParsed;
            await Task.Run(() => _scanner.RunScannerAsync());

            return true;
        }


        public event FileFoundEventHandler FileFound;
        public event ScanCompletedEventHandler ScanCompleted;

        private void ScannerFileParsed(AudioScan sender, ScanFileEventArgs e)
        {
            if (FileFound != null)
                FileFound(e);
        }

        private async void ScannerScanDone(AudioScan sender, ScanCompletedEventArgs e)
        {
            _lib = _scanner.MediaLibrary;
            await MediaRepository.Instance.Save(_lib);
            if (ScanCompleted != null)
                ScanCompleted(e);
        }
    }
}