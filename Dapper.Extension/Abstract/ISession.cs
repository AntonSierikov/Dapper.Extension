using Dapper.Extension.Enums;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dapper.Extension.Abstract
{
    public interface ISession : IDisposable
    {
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }
        DatabaseAccessor DatabaseAccessor { get; }
        SqlProvider SqlProvider { get; }

        Task<IDbConnection> OpenConnection(String connectionString);
        IDbTransaction OpenTransaction(IsolationLevel isolationlevel = IsolationLevel.ReadCommitted);

        ICommand<T, TKey> CreateBaseCommand<T, TKey>() where T : class;
        IQuery<T> CreateBaseQuery<T>() where T : class;

        TCustomCommandInterface GetCustomCommand<TCustomCommandInterface, TEntity, TCustomCommand>() where TCustomCommandInterface : class;
        TCustomQueryInteface GetCustomQuery<TCustomQueryInteface, TEntity, TCustomQuery>() where TCustomQueryInteface : class;
    }
}
