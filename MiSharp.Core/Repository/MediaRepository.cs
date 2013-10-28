using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DeadDog.Audio.Parsing.ID3;
using DeadDog.Audio.Scan;
using DeadDog.Audio.Scan.AudioScanner;
using MiSharp.Core.Library;

namespace MiSharp.Core.Repository
{
    public class MediaRepository : RepositoryBase
    {
        public delegate void FileFoundEventHandler(FileStatEventArgs e);

        public delegate void ScanCompletedEventHandler();


        private const string LibPath = "library.lib";
        private static MediaRepository _library;

        public MediaRepository() : base(LibPath)
        {                        
        }

        public static MediaRepository Instance
        {
            get { return _library ?? (_library = new MediaRepository()); }
        }

        public DeadDog.Audio.Libraries.Library GetLibrary()
        {            
            DeadDog.Audio.Libraries.Library lib = Repository.GetAll<DeadDog.Audio.Libraries.Library>().FirstOrDefault();
            //Repository.Activate(lib,20);
            return lib ?? new DeadDog.Audio.Libraries.Library();
        }

        private AudioScanner _scanner;
        private DeadDog.Audio.Libraries.Library _lib;
        public async Task<bool> Rescan()
        {
            _lib = GetLibrary();
            _scanner = new AudioScanner(new ID3Parser(), Settings.Instance.WatchFolder, SearchOption.AllDirectories);
            _scanner.MediaLibrary = _lib;
            _scanner.ScanDone += scanner_ScanDone;
            _scanner.FileParsed += scanner_FileParsed;
            await Task.Run(() => _scanner.RunScannerAsync());
            
            return true;
        }

        public event FileFoundEventHandler FileFound;
        public event ScanCompletedEventHandler ScanCompleted;
        void scanner_FileParsed(AudioScan sender, ScanFileEventArgs e)
        {
            //FileFound(new FileStatEventArgs());
        }

        private void scanner_ScanDone(AudioScan sender, ScanCompletedEventArgs e)
        {
            _lib = _scanner.MediaLibrary;
            Save(_lib);
            ScanCompleted();
        }
    }
}