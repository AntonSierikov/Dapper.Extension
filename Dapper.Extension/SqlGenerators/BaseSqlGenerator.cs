using Dapper.Extension.Constants;
using Dapper.Extension.Entities;
using Dapper.Extension.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public virtual String DeleteQuery(DatabaseTypeInfo databaseTypeInfo, Object obj)
        {
            String primaryKeyCondition = GetConditionByObject(databaseTypeInfo, obj);
            String deleteStatement = $@"DELETE FROM {databaseTypeInfo.TableDesignation} WHERE {primaryKeyCondition}";
            return deleteStatement;
        }

        //----------------------------------------------------------------//

        public virtual String GetEntityQuery(DatabaseTypeInfo databaseTypeInfo, Object parameters)
        {
            String primaryKeyCondition = GetConditionByObject(databaseTypeInfo, parameters);
            String getQuery = $@"SELECT * FROM {databaseTypeInfo.TableDesignation} WHERE {primaryKeyCondition}";
            return getQuery;
        }

        //----------------------------------------------------------------//

        public virtual String InsertQuery(DatabaseTypeInfo databaseTypeInfo)
        {
            IEnumerable<String> values = databaseTypeInfo.FieldColumnMap
                .Select(m => m.Value.IsNeedUseDefaultValue ? SqlKeywords.DEFAULT : $"@{m.Key}");
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

        protected String GetConditionByObject(DatabaseTypeInfo databaseTypeInfo, Object obj)
        {
            Type objType = obj.GetType();
            IEnumerable<ColumnDefinition> columnsDefinition = databaseTypeInfo.FieldColumnMap.Select(fc => fc.Value);

            IEnumerable<String> p_properties = objType.GetProperties().Select(p => p.Name);
            IEnumerable<String> P_fields = objType.GetFields().Select(f => f.Name);

            var customKeyMap = p_properties.Union(P_fields)
                .Join(columnsDefinition, p => p, c => c.PropertyName, (p, c) => new { PropertyOrFieldName = p, Column = c })
                .ToDictionary(p => p.PropertyOrFieldName, c => c.Column);

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
