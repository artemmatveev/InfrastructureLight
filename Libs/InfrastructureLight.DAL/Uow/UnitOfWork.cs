using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Linq;
using InfrastructureLight.DAL.Repositories;

namespace InfrastructureLight.DAL.Uow
{    
    using Common.Exceptions;    

    public class UnitOfWork : IUnitOfWork
    {
        DbContext _dataContext;
        IRepository[] _repositories;
        readonly object _locked = new object();

        public UnitOfWork(DbContext dataContext, params IRepository[] repositories)
        {
            var oldDataContexts = repositories.Select(repo
                                    => repo.GetType()
                                            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                            .FirstOrDefault(y => y.FieldType == typeof(DbContext))?.GetValue(repo))
                                  .OfType<DbContext>()
                                  .ToList();

            if (oldDataContexts.Any())
            {
                if (oldDataContexts.Any(x => !Equals(x, oldDataContexts[0])))
                {
                    throw new ArgumentOutOfRangeException(nameof(repositories),
                        new ErrorSingleContextException().Message);
                }
            }

            _dataContext = dataContext;
            _repositories = repositories;

            EFContextHelper.SetContext(_repositories, dataContext);
        }

        public virtual void SaveChanges()
        {
            try {
                lock (_locked) {                    
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

        public virtual bool HasChanges() 
            => _dataContext.ChangeTracker.HasChanges();        

        public virtual void Rollback()
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

        #region IDispose

        bool _disposed;
        public virtual void Dispose()
        {
            if (_disposed) { return; }

            foreach (var repo in _repositories) {
                repo.Dispose();
            }
            
            _dataContext?.Dispose();
            _dataContext = null;

            _disposed = true;
        }

        #endregion
    }
}
