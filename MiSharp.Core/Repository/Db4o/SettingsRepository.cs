namespace MiSharp.Core.Repository.Db4o
{
    public class SettingsRepository : RepositoryBase
    {
        private const string LibPath = "settings.msh";
        private static SettingsRepository _library;

        public SettingsRepository() : base(LibPath)
        {
        }

        public static SettingsRepository Instance
        {
            get { return _library ?? (_library = new SettingsRepository()); }
        }
    }
}