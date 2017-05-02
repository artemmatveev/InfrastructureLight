using System.Data;

namespace InfrastructureLight.SqlServer
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();

        string DataBaseName { get; }
        string ConnectionString { get; }
    }
}
