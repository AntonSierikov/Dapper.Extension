using Dapper.Extension.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Dapper.Extension.Helpers;

namespace Dapper.Extension.Entities
{
    public class DatabaseTypeInfo
    {
        public String TableSchema { get; }
        public String TableName { get; }
        internal Dictionary<String, ColumnDefinition> FieldColumnMap { get; }

        internal Dictionary<String, PrimaryKeyDefinition> FieldPrimaryKeyMap { get; }

        //----------------------------------------------------------------//

        public String TableDesignation
        {
            get
            {
                return !String.IsNullOrEmpty(TableSchema)
                       ? $"{TableSchema}.{TableName}"
                       : TableName;
            }
        }

        //----------------------------------------------------------------//

        public DatabaseTypeInfo(Type type)
        {
            PropertyInfo[] propertiesInfo = type.GetProperties();

            TableName = type.GetCustomAttribute<TableNameAttribute>().TableName;
            FieldColumnMap = TypeMapperHelper.GetFieldByPropertiesInfo(propertiesInfo);
            FieldPrimaryKeyMap = TypeMapperHelper.GetPrimaryKeyByPropertiesInfo(propertiesInfo);
            TableSchema = type.GetCustomAttribute<TableSchemaAttribute>()?.SchemaName;
        }

        //----------------------------------------------------------------//

    }
}
