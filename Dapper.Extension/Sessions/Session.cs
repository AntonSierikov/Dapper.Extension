using Dapper.Extension.Abstract;
using Dapper.Extension.Commands;
using Dapper.Extension.Enums;
using Dapper.Extension.Queries;
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

        public DatabaseAccessor DatabaseAccessor { get; }

        public SqlProvider SqlProvider { get; }

        //----------------------------------------------------------------//

        public Session(DatabaseAccessor dbAccessor, SqlProvider sqlProvider)
        {
            DatabaseAccessor = dbAccessor;
            SqlProvider = sqlProvider;
        }

        //----------------------------------------------------------------//

        public async Task<IDbConnection> OpenConnection(String connectionString)
        {
            Connection = DatabaseAccessor.ProviderFactories[SqlProvider].CreateConnection();
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

        public ICommand<T, TKey> CreateBaseCommand<T, TKey>() where T: class
        {
            return new BaseCommand<T, TKey>(this);
        }

        //----------------------------------------------------------------//
        
        public IQuery<T> CreateBaseQuery<T>() where T: class
        {
            return new BaseQuery<T>(this);
        }

        //----------------------------------------------------------------//

        public TCustomCommandInterface GetCustomCommand<TCustomCommandInterface, TEntity, TCustomCommand>() 
            where TCustomCommandInterface: class
        {
            return DatabaseAccessor.CustomCommands[typeof(TEntity).GetHashCode()] as TCustomCommandInterface;
        }

        public TCustomQueryInteface GetCustomQuery<TCustomQueryInteface, TEntity, TCustomQuery>()
            where TCustomQueryInteface: class
        {
            return DatabaseAccessor.CustomQueries[typeof(TEntity).GetHashCode()] as TCustomQueryInteface; 
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
