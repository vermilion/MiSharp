using System;
using System.Linq;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

namespace MiSharp.Core.Repository
{
    public class Db4ORepository : IRepository, IDisposable
    {
        public IEmbeddedObjectContainer Container;

        public Db4ORepository(string storagePath)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentActivationSupport());
            configuration.Common.Add(new TransparentPersistenceSupport());
            Container = Db4oEmbedded.OpenFile(configuration, storagePath);
        }

        public Db4ORepository(string storagePath, int activationDepth, int updateDepth)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ActivationDepth = activationDepth;
            configuration.Common.UpdateDepth = updateDepth;
            Container = Db4oEmbedded.OpenFile(configuration, storagePath);
        }

        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        public void Activate(object obj, int depth)
        {
            Container.Activate(obj, depth);
        }

        #region IRepository implementation

        public IQueryable<T> GetAll<T>()
        {
            return Container.AsQueryable<T>();
        }

        public async Task Save<T>(T entity)
        {
            await Task.Run(() => Container.Store(entity));
        }

        public async Task Delete<T>(T entity)
        {
            await Task.Run(() => Container.Delete(entity));
        }

        #endregion
    }
}