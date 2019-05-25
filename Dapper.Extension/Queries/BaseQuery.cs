using Dapper.Extension.Abstract;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Dapper.Extension.Queries
{
    class BaseQuery<T> : BaseDbOperation<T>, IQuery<T>
    {
        //----------------------------------------------------------------//

        public BaseQuery(ISession session)
            : base(session)
        {}

        //----------------------------------------------------------------//

        public Task<T> GetAsync(Object obj)
        {
            String getQuery = SqlGenerator.GetEntityQuery(DatabaseTypeInfo, obj);
            return Connection.QueryFirstOrDefaultAsync<T>(getQuery, obj, Transaction);
        }

        //----------------------------------------------------------------//
    }
}
