using Dapper.Extension.Abstract;
using Dapper.Extension.Enums;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dapper.Extension.Sessions
{
    internal class Session : ISession
    {

        //----------------------------------------------------------------//

        public DbConnection Connection { get; private set; }

        public DbTransaction Transaction { get; private set; }

        public DatabaseAccessor DatabaseEnvironment { get; }

        public SqlProvider SqlProvider { get; }

        //----------------------------------------------------------------//

        public Session(DatabaseAccessor dbEnvironment, SqlProvider sqlProvider)
        {
            DatabaseEnvironment = dbEnvironment;
            SqlProvider = sqlProvider;
        }

        //----------------------------------------------------------------//

        public async Task<IDbConnection> OpenConnection(String connectionString)
        {
            Connection = DatabaseEnvironment.ProviderFactories[SqlProvider].CreateConnection();
            Connection.ConnectionString = connectionString;
            await Connection.OpenAsync();
            return Connection;
        }

        //----------------------------------------------------------------//

        public IDbTransaction OpenTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Transaction = Connection.BeginTransaction(isolationLevel);
            return Transaction;
        }


        //----------------------------------------------------------------//

        public void Dispose()
        {
            CommitTransaction();
            CloseConneciton();
        }

        //----------------------------------------------------------------//

        private void CommitTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction = null;
            }
        }

        //----------------------------------------------------------------//

        private void CloseConneciton()
        {
            if(Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }

        //----------------------------------------------------------------//

    }
}
