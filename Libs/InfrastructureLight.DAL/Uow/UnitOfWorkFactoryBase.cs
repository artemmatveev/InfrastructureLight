using System.Data.Entity;
using InfrastructureLight.DAL.Repositories;

namespace InfrastructureLight.DAL.Uow
{
    public abstract class UnitOfWorkFactoryBase : IUnitOfWorkFactory
    {
        readonly DbContext _dbContext;
        protected UnitOfWorkFactoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork Create(params IRepository[] repositories)
        {
            return new UnitOfWork(_dbContext, repositories);
        }
    }
}
