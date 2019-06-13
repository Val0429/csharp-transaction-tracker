using System.Data.Common;

namespace Constant.Data
{
    public abstract class DbProvider : DbProviderFactory
    {
        protected readonly string ConnectionString;


        // Constructor
        protected DbProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract DbParameter CreateParameter(string key, object val);

        public abstract DataAdapter CreateDataAdapter(DbCommand command);
    }

    public interface IDbProviderFactory
    {
        DbProvider CreateDbProvider();
    }
}