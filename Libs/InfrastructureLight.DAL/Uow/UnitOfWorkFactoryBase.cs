using System.Data.Entity;
using InfrastructureLight.DAL.Repositories;

namespace InfrastructureLight.DAL.Uow
{
    public abstract class UnitOfWorkFactoryBase : IUnitOfWorkFactory
    {
        protected DbContext _dbContext;        
        public IUnitOfWork Create(params IRepository[] repositories)
        {
            ReloadContext();
            return new UnitOfWork(_dbContext, repositories);
        }

        public abstract void ReloadContext();
    }
}
