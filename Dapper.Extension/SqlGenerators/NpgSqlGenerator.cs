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

        public override string InsertQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            String comma = CharConstants.COMMA.ToString();

            IEnumerable<String> values = databaseTypeInfo.FieldColumnMap
                .Select(m => m.Value.IsNeedUseDefaultValue ? SqlKeywords.DEFAULT : m.Key);
            IEnumerable<String> columns = databaseTypeInfo.FieldColumnMap.Select(m => m.Value.ColumnName);

            IEnumerable<String> keys = databaseTypeInfo.FieldPrimaryKeyMap.Select(k => k.Value.ColumnName);

            String query = $@"INSERT INTO {databaseTypeInfo.TableDesignation} 
                              ({String.Join(comma, columns)})
                              VALUES ({String.Join(comma, values)} 
                              OUTPUT ({String.Join(comma, keys)}";
            return query;
        }

        //----------------------------------------------------------------//

    }
}
