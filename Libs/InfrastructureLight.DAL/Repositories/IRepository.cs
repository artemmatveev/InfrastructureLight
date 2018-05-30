using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public interface IRepository<out TEntity> : IDisposable where TEntity : IEntity {

        IQueryable<TEntity> AsNoTracking();
        IQueryable<T> Execute<T>(string sql, params object[] parameters);               
    }
}
