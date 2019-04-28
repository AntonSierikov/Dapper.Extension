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
        private readonly BaseSqlGenerator _sqlGenerator;

        //----------------------------------------------------------------//

        public BaseCommand(IDbConnection connection)
            : base(connection)
        {
        }

        //----------------------------------------------------------------//

        public BaseCommand(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {}
        
        //----------------------------------------------------------------//

        public virtual Task<Int32> DeleteAsync(TKey key)
        {
            return Connection.ExecuteAsync(_sqlGenerator.DeleteQuery(DatabaseTypeInfo));
        }

        //----------------------------------------------------------------//

        public virtual Task<TKey> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        //----------------------------------------------------------------//

        public virtual Task<Int32> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        //----------------------------------------------------------------//

    }
}
