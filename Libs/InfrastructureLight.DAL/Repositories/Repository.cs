using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public abstract class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity, new()
    {
        protected DbContext _dataContext;                
        IDbSet<TEntity> _entities;
        
        #region IRepository

        protected IDbSet<TEntity> Entities 
            => _entities ?? (_entities = _dataContext.Set<TEntity>());

        public IQueryable<TEntity> AsNoTracking() 
            => Entities.AsNoTracking();

        public IQueryable<T> Execute<T>(string sql, params object[] parameters)
            => _dataContext.Database.SqlQuery<T>(sql, parameters).AsQueryable();

        protected IQueryable<TEntity> Get(bool asNoTrackingFlag = false, Expression<Func<TEntity, bool>> condition = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> set = asNoTrackingFlag ? AsNoTracking() : Entities;
            set = includes.Aggregate(set, (current, include) => current.Include(include));

            if (condition != null) {
                set = set.Where(condition);
            }

            return set;
        }
                
        protected virtual TEntity Create()
        {
            _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());
            return _entities.Create<TEntity>();
        }

        protected virtual void Delete(IEnumerable<TEntity> entities)
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
            if (trEntity != null) { _entities.Add(entity); }
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
