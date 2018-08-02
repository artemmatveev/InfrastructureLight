using System;
using System.Linq;

namespace InfrastructureLight.DAL.Repositories
{
    using Domain.Interfaces;

    public interface IRepository : IDisposable { }
    public interface IRepository<out TEntity> : IRepository where TEntity : IEntity {
        IQueryable<TEntity> AsNoTracking();
        IQueryable<T> Execute<T>(string sql, params object[] parameters);               
    }
}
