using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Extension.Constants;
using Dapper.Extension.Entities;

namespace Dapper.Extension.SqlGenerators
{
    internal class NpgSqlGenerator : BaseSqlGenerator
    {

        //----------------------------------------------------------------//

        protected override string InsertQuery(
            String tableDesignation,
            IEnumerable<String> values, 
            IEnumerable<String> columns, 
            IEnumerable<String> keys)
        {
            String query = $@"INSERT INTO {tableDesignation} 
                              ({String.Join(StringConstants.COMMA, columns)})
                              VALUES ({String.Join(StringConstants.COMMA, values)}) 
                              RETURNING ({String.Join(StringConstants.COMMA, keys)})";
            return query;
        }

        //----------------------------------------------------------------//

    }
}
