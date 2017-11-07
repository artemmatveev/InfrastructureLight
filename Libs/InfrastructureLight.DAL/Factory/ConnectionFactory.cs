using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Global.Security;

namespace InfrastructureLight.DAL.Factory
{
    public class ConnectionFactory : IConnectionFactory
    {
        #region Fields

        public string DataBaseName { get; private set; }
        public string ConnectionString { get; private set; }

        #endregion

        public ConnectionFactory(string cryptKey, string name)
        {
            var cryptConnection = ConfigurationManager.ConnectionStrings[name];
            var dbName = ConfigurationManager.ConnectionStrings["DataBaseName"];

            if (cryptConnection != null)
            {
                var encryptConnection = new SqlConnectionStringBuilder(Crypto.DecryptString(cryptConnection.ConnectionString, cryptKey));
                DataBaseName = (dbName == null) ? string.Empty : dbName.ConnectionString;

                if (!string.IsNullOrEmpty(DataBaseName))
                {
                    encryptConnection.InitialCatalog = DataBaseName;
                }

                ConnectionString = encryptConnection.ConnectionString;
            }
        }

        public IDbConnection CreateConnection()
        {
            SqlConnection sqlConnection = string.IsNullOrEmpty(ConnectionString)
                ? new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
                : new SqlConnection(ConnectionString);

            return sqlConnection;
        }
    }
}
