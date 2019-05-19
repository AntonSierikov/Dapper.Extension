using Dapper.Extension.Abstract;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Dapper.Extension.Queries
{
    class BaseQuery<T, TKey> : BaseDbOperation<T>, IQuery<T, TKey>
    {
        //----------------------------------------------------------------//

        public BaseQuery(ISession session)
            : base(session)
        {}

        //----------------------------------------------------------------//

        public Task<T> GetAsync(TKey key)
        {
            String getQuery = SqlGenerator.GetEntityQuery(DatabaseTypeInfo, key);
            return Connection.QueryFirstOrDefaultAsync<T>(getQuery, key, Transaction);
        }

        //----------------------------------------------------------------//
    }
}
