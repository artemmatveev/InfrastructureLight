using System.Data;

namespace InfrastructureLight.DAL.Factory
{
    public interface IConnectionManager
    {
        IDbConnection CreateConnection();

        string DataBaseName { get; }
        string ConnectionString { get; }
    }
}
