using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data;
using System.Data.Common;
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

        public ConnectionFactory(string cryptKey)
        {
            var cryptConnection = ConfigurationManager.ConnectionStrings["CryptConnectionString"];
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
