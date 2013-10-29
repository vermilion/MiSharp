using System.Linq;
using DeadDog.Audio.Libraries;

namespace MiSharp.Core.Repository
{
    public class MediaRepository : RepositoryBase
    {
        private const string LibPath = "library.lib";
        private static MediaRepository _instance;

        public MediaRepository() : base(LibPath)
        {
        }

        public static MediaRepository Instance
        {
            get { return _instance ?? (_instance = new MediaRepository()); }
        }

        public Library GetLibrary()
        {
            return Repository.GetAll<Library>().FirstOrDefault() ?? new Library();
        }
    }
}