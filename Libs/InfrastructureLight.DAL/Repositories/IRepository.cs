using System;
using System.Collections.Generic;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public interface IRepository : IDisposable { }
    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity {
        TEntity Create();
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
        void AddOrUpdate(TEntity entity);
        void Attach(TEntity entity);
        void Reload(TEntity entity);
        void Detach(TEntity entity);
    }
}
