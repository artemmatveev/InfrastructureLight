using InfrastructureLight.DAL.Repositories;
using System.Data.Entity;

namespace InfrastructureLight.BLL.Uow
{
    public abstract class UnitOfWorkFactoryBase : IUnitOfWorkFactory
    {
        public IUnitOfWork Create(params IRepository[] repositories)
        {
            var context = ReloadContext();
            return new UnitOfWork(context, repositories);
        }

        protected abstract DbContext ReloadContext();
    }
}
