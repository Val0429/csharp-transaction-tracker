using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security;
using System.Security.Permissions;

namespace Constant.Data
{
    public class SqlProvider : DbProvider
    {
        // Field
        private readonly DbProviderFactory _dbProviderFactory = SqlClientFactory.Instance;

        /// <summary>
        /// Create a DbProvider using Standard Security connection
        /// </summary>
        public SqlProvider(string address, string dbName, string userId, string password, string instanceName = null)
            : this(CreateConnectionString(address, dbName, userId, password, instanceName))
        {

        }

        public SqlProvider(string address, int port, string dbName, string userId, string password, string instanceName = null)
            : this(CreateConnectionString(string.Format("{0},{1}", address, port), dbName, userId, password, instanceName))
        {

        }

        /// <summary>
        /// Create a DbProvider using trusted connection
        /// </summary>
        public SqlProvider(string dbName, string address, string instanceName = null)
            : this(CreateTrustedConnection(dbName, address, instanceName))
        {

        }

        public SqlProvider(string dbName, string address, int port, string instanceName = null)
            : this(CreateTrustedConnection(dbName, string.Format("{0},{1}", address, port), instanceName))
        {

        }

        public SqlProvider(string connectionString)
            : base(connectionString)
        {

        }

        private static string CreateTrustedConnection(string dbName, string address, string instanceName)
        {
            // Persist Security Info=False;Integrated Security=true;Initial Catalog=Northwind;server=(local)
            return string.IsNullOrEmpty(instanceName)
                ? string.Format("Persist Security Info=False;Integrated Security=true;Initial Catalog={1};server={0}", address, dbName)
                : string.Format(@"Persist Security Info=False;Integrated Security=true;Initial Catalog={1};server={0}\{2}", address, dbName, instanceName);
            //? String.Format("Server={0};Database={1};Trusted_Connection=True;", address, dbName)
            //: String.Format(@"Server={0}\{2};Database={1};Trusted_Connection=True;", address, dbName, instanceName);
        }

        private static string CreateConnectionString(string address, string dbName, string userId, string password,
            string instanceName)
        {
            return string.IsNullOrEmpty(instanceName)
                ? string.Format("Server={0};Database={1};User Id={2};Password={3};", address, dbName, userId, password)
                : string.Format(@"Server={0}\{4};Database={1};User Id={2};Password={3};", address, dbName, userId, password, instanceName);
        }

        public override DbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public override DbParameter CreateParameter(string key, object val)
        {
            return new SqlParameter(key, val);
        }

        public override DataAdapter CreateDataAdapter(DbCommand command)
        {
            var sqlCommand = command as SqlCommand;

            return new SqlDataAdapter(sqlCommand);
        }

        #region DbProviderFactory
        public override bool CanCreateDataSourceEnumerator
        {
            get { return _dbProviderFactory.CanCreateDataSourceEnumerator; }
        }

        public override DbCommand CreateCommand()
        {
            return _dbProviderFactory.CreateCommand();
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return _dbProviderFactory.CreateCommandBuilder();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return _dbProviderFactory.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return _dbProviderFactory.CreateDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return _dbProviderFactory.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return _dbProviderFactory.CreateParameter();
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            return _dbProviderFactory.CreatePermission(state);
        }
        #endregion
    }
}