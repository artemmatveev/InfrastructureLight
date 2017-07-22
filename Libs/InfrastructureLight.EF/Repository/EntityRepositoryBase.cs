using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

using InfrastructureLight.Domain;

namespace InfrastructureLight.EF.Repository
{
    public class EntityRepositoryBase : IEntityRepository
    {
        private readonly object _locked = new object();
        private DbContext _dataContext;

        public EntityRepositoryBase(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region IEntityRepository

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> condition = null, bool trackingFlag = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            lock (_locked)
            {
                IQueryable<TEntity> set = trackingFlag
                    ? _dataContext.Set<TEntity>().AsNoTracking()
                    : _dataContext.Set<TEntity>();

                set = includes.Aggregate(set, (current, include) => current.Include(include));

                if (condition != null)
                    set = set.Where(condition);

                return set;
            }
        }

        public virtual IQueryable<TEntity> Execute<TEntity>(string sqlQuery, params object[] parameters)
        {
            return _dataContext.Database.SqlQuery<TEntity>(sqlQuery, parameters).AsQueryable();
        }

        public TEntity Create<TEntity>() where TEntity : class
        {
            lock (_locked)
            {
                return _dataContext.Set<TEntity>().Create<TEntity>();
            }
        }

        public void AddOrUpdate(Entity entity)
        {
            lock (_locked)
            {
                DbSet dbSet = _dataContext.Set(entity.GetType());
                if (entity.IsTransient)
                {
                    dbSet.Add(entity);
                }
            }
        }

        public void Delete(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                entity.Delete();
            }
        }

        public void Delete(Entity entity)
        {
            Delete(new[] { entity });
        }

        public void Commit()
        {
            lock (_locked)
            {
                _dataContext.SaveChanges();
            }
        }

        public void Rollback()
        {
            lock (_locked)
            {
                foreach (DbEntityEntry entry in _dataContext.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
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

        public virtual void Reload()
        {
            lock (_locked)
            {

            }
        }

        public bool HasChanges()
        {
            lock (_locked)
            {
                return _dataContext.ChangeTracker.HasChanges();
            }
        }

        #endregion
    }
}
