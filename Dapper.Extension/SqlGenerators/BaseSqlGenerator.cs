using Dapper.Extension.Constants;
using Dapper.Extension.Entities;
using Dapper.Extension.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.Extension.SqlGenerators
{
    internal abstract class BaseSqlGenerator
    {
        #region Public Methods

        //----------------------------------------------------------------//

        public virtual String UpdateQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            String fieldColumnUpdateAStatement = String.Join(CharConstants.COMMA.ToString(), databaseTypeInfo.FieldColumnMap.Select(fc => $"{fc.Value.ColumnName} = @{fc.Key}"));
            String primaryKeyCondition = GetPrimaryKeyCondition(databaseTypeInfo);
            String updateStatement = $@"UPDATE {databaseTypeInfo.TableDesignation} SET
                                        {fieldColumnUpdateAStatement}
                                        WHERE {primaryKeyCondition}";
            return updateStatement;
        }

        //----------------------------------------------------------------//

        public virtual String DeleteQuery(DatabaseTypeInfo databaseTypeInfo, Object key)
        {
            String primaryKeyCondition = GetPrimaryKeyCondition(databaseTypeInfo, key);
            String deleteStatement = $@"DELETE FROM {databaseTypeInfo.TableDesignation} WHERE {primaryKeyCondition}";
            return deleteStatement;
        }

        //----------------------------------------------------------------//

        public virtual String GetEntityQuery(DatabaseTypeInfo databaseTypeInfo, Object key)
        {
            String primaryKeyCondition = GetPrimaryKeyCondition(databaseTypeInfo);
            String getQuery = $@"SELECT * FROM {databaseTypeInfo.TableDesignation} WHERE {primaryKeyCondition}";
            return getQuery;
        }

        //----------------------------------------------------------------//

        public virtual String InsertQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            IEnumerable<String> values = databaseTypeInfo.FieldColumnMap
                .Select(m => m.Value.IsNeedUseDefaultValue ? SqlKeywords.DEFAULT : m.Key);
            IEnumerable<String> columns = databaseTypeInfo.FieldColumnMap.Select(m => m.Value.ColumnName);
            IEnumerable<String> keys = databaseTypeInfo.FieldPrimaryKeyMap.Select(k => k.Value.ColumnName);
            return InsertQuery(databaseTypeInfo.TableDesignation, values, columns, keys);
        }

        //----------------------------------------------------------------//

        #endregion

        #region Protected methods 

        protected String GetPrimaryKeyCondition(DatabaseTypeInfo databaseTypeInfo)
        {
            return OperatorHelper.JoinPropertyColumn(
                databaseTypeInfo.FieldPrimaryKeyMap,
                ComparisonOperators.EQUAL.ToString(),
                LogicalOperators.AND);
        }

        //----------------------------------------------------------------//

        protected String GetPrimaryKeyCondition(DatabaseTypeInfo databaseTypeInfo, Object key)
        {
            Type keyType = key.GetType();
            IEnumerable<PrimaryKeyDefinition> columns = databaseTypeInfo.FieldPrimaryKeyMap.Select(fc => fc.Value);
            var customKeyMap = keyType.GetProperties().Join(columns, p => p.Name, c => c.PropertyName,
                                    (p, c) => new { Property = p, Column = c }).ToDictionary(p => p.Property.Name, c => c.Column);
            return OperatorHelper.JoinPropertyColumn(customKeyMap, ComparisonOperators.EQUAL.ToString(), LogicalOperators.AND);
        }

        //----------------------------------------------------------------//

        protected abstract String InsertQuery(
            String tableDesignation,
            IEnumerable<String> values,
            IEnumerable<String> columns,
            IEnumerable<String> keys);

        //----------------------------------------------------------------//

        #endregion

    }
}
