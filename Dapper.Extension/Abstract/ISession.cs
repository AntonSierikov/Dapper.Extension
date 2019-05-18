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
        DatabaseAccessor DatabaseEnvironment { get; }
        SqlProvider SqlProvider { get; }

        Task<IDbConnection> OpenConnection(String connectionString);
        IDbTransaction OpenTransaction(IsolationLevel isolationlevel = IsolationLevel.ReadCommitted);
    }
}
