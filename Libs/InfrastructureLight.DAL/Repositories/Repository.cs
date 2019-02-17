using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public abstract class Repository : IRepository
    {
        protected DbContext _dataContext;

        protected IQueryable<T> Execute<T>(string sql, params object[] parameters)
            => _dataContext.Database.SqlQuery<T>(sql, parameters).AsQueryable();

        #region Dispose

        bool _disposed;
        public void Dispose()
        {
            if (_disposed) { return; }
            _disposed = true;
        }

        #endregion
    }

    public abstract class Repository<TEntity> : Repository, IRepository<TEntity>
        where TEntity : class, new()
    {
        protected IDbSet<TEntity> Entities
            => _dataContext.Set<TEntity>();

        protected IQueryable<TEntity> AsNoTracking()
            => Entities.AsNoTracking();

        protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> condition = null, bool asNoTrackingFlag = false, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> set = Entities;
            set = includes.Aggregate(set, (current, include) => current.Include(include));

            if (condition != null)
            {
                set = set.Where(condition);
            }

            if (asNoTrackingFlag)
            {
                set = set.AsNoTracking();
            }

            return set;
        }

        #region IRepository

        public virtual TEntity Create()
        {
            return Entities.Create<TEntity>();
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                var sdEntity = entity as ISoftDeletedEntity;
                if (sdEntity != null)
                {
                    var trEntity = entity as ITransientEntity;
                    if (trEntity != null)
                    {
                        if (trEntity.IsTransient)
                        {
                            Entities.Remove(entity);
                        }
                        else
                        {
                            sdEntity.Delete();
                        }
                    }
                    else
                    {
                        Entities.Remove(entity);
                    }
                }
                else
                {
                    Entities.Remove(entity);
                }
            }
        }

        public virtual void Delete(TEntity entity)
        {
            Delete(new[] { entity });
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            var trEntity = entity as ITransientEntity;
            if (trEntity != null && trEntity.IsTransient) { Entities.Add(entity); }
        }

        public virtual void Attach(TEntity entity)
        {            
            Entities.Attach(entity);
        }

        public virtual void Detach(TEntity entity)
        {
            ((IObjectContextAdapter)_dataContext).ObjectContext.Detach(entity);
        }

        public virtual void Reload(TEntity entity)
        {
            _dataContext.Entry(entity).Reload();
        }

        #endregion

        #region IDispose

        bool _disposed;
        public new void Dispose()
        {
            if (_disposed) { return; }
            
            _disposed = true;
        }

        #endregion
    }
}
