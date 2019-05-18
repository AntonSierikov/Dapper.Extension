using Dapper.Extension.Entities;
using Dapper.Extension.SqlGenerators;
using System.Data;

namespace Dapper.Extension.Abstract
{
    public abstract class BaseDbOperation<T>
    {
        protected DatabaseTypeInfo DatabaseTypeInfo { get; }
        protected IDbConnection Connection { get; }
        protected IDbTransaction Transaction { get; }
        private protected BaseSqlGenerator SqlGenerator { get; }

        //----------------------------------------------------------------//

        public BaseDbOperation(ISession session)
        {
            Connection = session.Connection;
            Transaction = session.Transaction;
            DatabaseTypeInfo = session.DatabaseEnvironment.DatabaseEntitiesInfo[typeof(T).GetHashCode()];
            SqlGenerator = session.DatabaseEnvironment.SqlGenerators[session.SqlProvider];
        }

        //----------------------------------------------------------------//

    }
}
