using Dapper.Extension.Entities;
using Dapper.Extension.Providers;
using System.Data;

namespace Dapper.Extension.Abstract
{
    public abstract class BaseDbOperation<T>
    {
        protected DatabaseTypeInfo DatabaseTypeInfo { get; }
        protected IDbConnection Connection { get; }
        protected IDbTransaction Transaction { get; }

        //----------------------------------------------------------------//

        public BaseDbOperation(IDbConnection connection)
        {
            DatabaseTypeInfo = DatabaseEntitiesInfoProvider.GetDatabaseEntityInfo<T>();
            Connection = connection;
        }

        //----------------------------------------------------------------//

        public BaseDbOperation(IDbConnection connection, IDbTransaction transaction)
            : this(connection)
        {
            Transaction = transaction;
        }

        //----------------------------------------------------------------//

    }
}
