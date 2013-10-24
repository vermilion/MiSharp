using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MiSharp.Core.Repository
{
    public abstract class RepositoryBase : IDisposable
    {
        private readonly string _libPath;
        public Db4ORepository Repository;

        protected RepositoryBase(string libPath)
        {
            _libPath = libPath;
            Initialize();
        }

        protected void Initialize()
        {
            Repository = new Db4ORepository(_libPath);
        }

        public void Recreate()
        {
            Repository.Dispose();
            File.WriteAllText(_libPath, "");
            Initialize();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return Repository.GetAll<T>();
        }

        public async Task Save<T>(T item)
        {
            await Repository.Save(item);
        }

        public async Task Delete<T>(T item)
        {
            await Repository.Delete(item);
        }

        #region IDisposable implementation

        public void Dispose()
        {
            if (Repository != null)
                Repository.Dispose();
        }

        #endregion
    }
}