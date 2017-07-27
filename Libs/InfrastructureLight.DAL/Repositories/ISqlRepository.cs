using System;
using System.Collections.Generic;
using System.Data;

namespace InfrastructureLight.DAL.Repositories
{
    public interface ISqlRepository
    {
        List<TEntity> Get<TEntity>(string query, Action<TEntity, IDataRecord> fill) where TEntity : new();
    }
}
