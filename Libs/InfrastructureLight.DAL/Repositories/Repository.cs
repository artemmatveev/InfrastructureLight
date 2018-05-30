using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public abstract class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity, new()
    {
        protected DbContext _dataContext;        
        protected readonly object _locked = new object();
        IDbSet<TEntity> _entities;

        public Repository(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region IRepository

        protected IDbSet<TEntity> Entities 
            => _entities ?? (_entities = _dataContext.Set<TEntity>());

        public IQueryable<TEntity> AsNoTracking() 
            => Entities.AsNoTracking();

        public IQueryable<T> Execute<T>(string sql, params object[] parameters)
            => _dataContext.Database.SqlQuery<T>(sql, parameters).AsQueryable();

        protected IQueryable<TEntity> Get(bool AsNoTrackingFlag = false, Expression<Func<TEntity, bool>> condition = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> set = AsNoTrackingFlag ? AsNoTracking() : Entities;
            set = includes.Aggregate(set, (current, include) => current.Include(include));

            if (condition != null) {
                set = set.Where(condition);
            }

            return set;
        }

        protected bool HasChanges() => _dataContext.ChangeTracker.HasChanges();

        protected void Rollback()
        {
            lock (_locked)
            {
                foreach (DbEntityEntry entry in _dataContext.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Detached: break;
                        case EntityState.Unchanged: break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
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

        public virtual void Delete(TEntity entity)
        {
            Delete(new[] { entity });
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            _entities = _entities ?? (_entities = _dataContext.Set<TEntity>());

            var trEntity = entity as ITransientEntity;
            if (trEntity != null) { _entities.Add(entity); }
        }

        public virtual void SaveChanges()
        {
            try {

                lock (_locked) {
                    var login = Environment.GetCommandLineArgs().Length > 1
                        ? Environment.GetCommandLineArgs()[1]
                        : "noname";

                    foreach (DbEntityEntry<TEntity> entry in _dataContext.ChangeTracker.Entries<TEntity>()) {
                        var _luEntity = entry.Entity as ILastUpdatedEntity;
                        if (_luEntity != null) {
                            _luEntity.ModifyDate = DateTime.Now;
                            _luEntity.ModifyBy = login;
                        }
                    }

                    _dataContext.SaveChanges();
                }                
            }
            catch (DbUpdateConcurrencyException ex) {
                throw;
            }
            catch (Exception ex) {
                throw;
            }
        }

        #endregion

        #region Dispose

        bool _disposed;
        public void Dispose()
        {
            if (_disposed) { return; }

            _entities = null;
            _dataContext?.Dispose();
            _dataContext = null;

            _disposed = true;
        }
        
        #endregion
    }
}
