using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using NLog;

namespace InfrastructureLight.DAL.Repository
{
    using Factory;

    public abstract class SqlRepositoryBase : ISqlRepository
    {
        private IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        public SqlRepositoryBase(IConnectionFactory connectionFactory, ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        #region Members ISqlRepository

        public List<TEntity> Get<TEntity>(string query, Action<TEntity, IDataRecord> fill) where TEntity : new()
        {
            if (string.IsNullOrEmpty(query)) { throw new ArgumentNullException(); }

            var result = new List<TEntity>();
            try
            {
                using (var cn = (SqlConnection)_connectionFactory.CreateConnection())
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        cmd.Connection = cn;
                        cn.Open();

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new TEntity();
                                fill(item, dr);
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return result;
        }

        #endregion
    }
}
