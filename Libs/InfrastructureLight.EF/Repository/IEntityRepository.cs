using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using InfrastructureLight.Domain;

namespace InfrastructureLight.EF.Repository
{
    public interface IEntityRepository
    {
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> condition = null, bool trackingFlag = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class;

        IQueryable<TEntity> Execute<TEntity>(string sqlQuery, params object[] parameters);

        TEntity Create<TEntity>() where TEntity : class;

        void AddOrUpdate(Entity entity);

        void Delete(IEnumerable<Entity> entities);

        void Delete(Entity entity);

        void Commit();

        void Rollback();

        void Reload();

        bool HasChanges();
    }
}
