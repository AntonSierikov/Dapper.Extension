using Dapper.Extension.Constants;
using Dapper.Extension.Entities;
using Dapper.Extension.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.Extension.SqlGenerators
{
    internal abstract class BaseSqlGenerator
    {

        //----------------------------------------------------------------//

        public abstract String InsertQuery(DatabaseTypeInfo databaseTypeInfo);

        //----------------------------------------------------------------//

        public virtual String UpdateQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            String fieldColumnUpdateAStatement = String.Join(CharConstants.COMMA.ToString(), databaseTypeInfo.FieldColumnMap.Select(fc => $"{fc.Value} = @{fc.Key}"));
            String primaryKeyCondition = GetPrimaryKeyCondition(databaseTypeInfo);
            String updateStatement = $@"UPDATE {databaseTypeInfo.TableName} SET
                                        {fieldColumnUpdateAStatement}
                                        WHERE {primaryKeyCondition}";
            return updateStatement;
        }

        //----------------------------------------------------------------//

        public virtual String DeleteQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            String primaryKeyCondition = GetPrimaryKeyCondition(databaseTypeInfo);
            String deleteStatement = $@"DELETE FROM {databaseTypeInfo.TableName} WHERE {primaryKeyCondition}";
            return deleteStatement;
        }

        //----------------------------------------------------------------//

        private String GetPrimaryKeyCondition(DatabaseTypeInfo databaseTypeInfo)
        {
            return OperatorHelper.JoinProperyColumn(databaseTypeInfo.FieldPrimaryKeyMap,
                                                    ComparisonOperators.EQUAL.ToString(),
                                                    LogicalOperators.AND);
        }

        //----------------------------------------------------------------//

        private String GetPrimaryKeyCondition(DatabaseTypeInfo databaseTypeInfo, Object key)
        {
            Type keyType = key.GetType();
            IEnumerable<String> columns = databaseTypeInfo.FieldColumnMap.Select(fc => fc.Value);
            var customKeyMap = keyType.GetProperties().Join(columns, p => p.Name, c => c,
                                    (p, c) => new { Property = p, Column = c }).ToDictionary(p => p.Property.Name, c => c.Column);
            return OperatorHelper.JoinProperyColumn(customKeyMap, ComparisonOperators.EQUAL.ToString(), LogicalOperators.AND);

        }

        //----------------------------------------------------------------//

    }
}
