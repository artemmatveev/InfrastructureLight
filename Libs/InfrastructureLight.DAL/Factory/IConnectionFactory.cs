using System.Data;

namespace InfrastructureLight.DAL.Factory
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();

        string DataBaseName { get; }
        string ConnectionString { get; }
    }
}
