using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Extension.Constants;
using Dapper.Extension.Entities;

namespace Dapper.Extension.SqlGenerators
{
    internal class SqlClientGenerator : BaseSqlGenerator
    {

        //----------------------------------------------------------------//

        protected override String InsertQuery(
            String tableDesignation,
            IEnumerable<String> values, 
            IEnumerable<String> columns, 
            IEnumerable<String> keys)
        {
            IEnumerable<String> insertedKeys = keys.Select(k => $"{SqlKeywords.INSERTED}.{k}");

            String query = $@"INSERT INTO {tableDesignation}
                              OUTPUT {String.Join(StringConstants.COMMA, insertedKeys)}
                              ({String.Join(StringConstants.COMMA, columns)}
                              VALUES ({String.Join(StringConstants.COMMA, values)}";

            return query;
        }

        //----------------------------------------------------------------//

    }
}
