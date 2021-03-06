﻿using InfrastructureLight.DAL.Repositories;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace InfrastructureLight.BLL.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        DbContext _dataContext;
        readonly IRepository[] _repositories;
        readonly object _locked = new object();

        public UnitOfWork(DbContext dataContext, params IRepository[] repositories)
        {
            _dataContext = dataContext;
            _repositories = repositories;

            EFContextHelper.SetContext(_repositories, dataContext);
        }

        public virtual void SaveChanges()
        {
            try
            {
                lock (_locked)
                {
                    _dataContext.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("Ошибка при сохранении", ex);
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
                            throw new InvalidOperationException();
                    }
                }
            }
        }
        
        #region IDispose

        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                foreach (var repo in _repositories) { repo.Dispose(); }

                _dataContext?.Dispose();
                _dataContext = null;
            }

            _disposed = true;
        }
        
        #endregion
    }
}
