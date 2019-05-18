using Dapper.Extension.Abstract;
using Dapper.Extension.SqlGenerators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extension.Commands
{
    public class BaseCommand<T, TKey> : BaseDbOperation<T>, ICommand<T, TKey> where T : class
    {
        //----------------------------------------------------------------//

        public BaseCommand(ISession session)
            : base(session)
        {}
        
        //----------------------------------------------------------------//

        public virtual Task<Int32> DeleteAsync(TKey key)
        {
            return Connection.ExecuteAsync(SqlGenerator.DeleteQuery(DatabaseTypeInfo, key));
        }

        //----------------------------------------------------------------//

        public virtual Task<TKey> InsertAsync(T entity)
        {
            return Connection.QueryFirstOrDefaultAsync<TKey>(SqlGenerator.InsertQuery(DatabaseTypeInfo));
        }

        //----------------------------------------------------------------//

        public virtual Task<Int32> UpdateAsync(T entity)
        {
            return Connection.ExecuteAsync(SqlGenerator.UpdateQuery(DatabaseTypeInfo));
        }

        //----------------------------------------------------------------//

    }
}
