using System;
using System.Collections.Generic;
using System.Data;

namespace InfrastructureLight.DAL.Repository
{
    public interface ISqlRepository
    {
        List<TEntity> Get<TEntity>(string query, Action<TEntity, IDataRecord> fill) where TEntity : new();
    }
}
