using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public abstract class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity, new()
    {
        protected DbContext _dataContext;                
        IDbSet<TEntity> _entities;
        
        protected IDbSet<TEntity> Entities
            => _entities ?? (_entities = _dataContext.Set<TEntity>());

        protected IQueryable<TEntity> AsNoTracking()
            => Entities.AsNoTracking();

        protected IQueryable<T> Execute<T>(string sql, params object[] parameters)
            => _dataContext.Database.SqlQuery<T>(sql, parameters).AsQueryable();

        protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> condition = null, bool asNoTrackingFlag = false, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> set = asNoTrackingFlag ? AsNoTracking() : Entities;
            set = includes.Aggregate(set, (current, include) => current.Include(include));

            if (condition != null)
            {
                set = set.Where(condition);
            }

            return set;
        }

        #region IRepository

        public virtual TEntity Create()
        {
            _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());
            return _entities.Create<TEntity>();
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());
                               
                var sdEntity = entity as ISoftDeletedEntity;
                if (sdEntity != null) {
                    var trEntity = entity as ITransientEntity;
                    if (trEntity != null) {
                        if (trEntity.IsTransient) {
                            _entities.Remove(entity);
                        }
                        else {
                            sdEntity.Delete();
                        }
                    }
                    else {
                        _entities.Remove(entity);
                    }
                }
                else {
                    _entities.Remove(entity);
                }                               
            }
        }

        public virtual void Delete(TEntity entity) {
            Delete(new[] { entity });
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());
            
            var trEntity = entity as ITransientEntity;
            if (trEntity != null && trEntity.IsTransient) { _entities.Add(entity); }            
        }

        public virtual void Attach(TEntity entity)
        {
            _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());
            _entities.Attach(entity);
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

        #region Dispose

        bool _disposed;
        public void Dispose()
        {
            if (_disposed) { return; }

            _entities = null;            
            _disposed = true;
        }
        
        #endregion
    }
}
