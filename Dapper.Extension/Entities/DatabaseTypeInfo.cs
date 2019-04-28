using Dapper.Extension.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Dapper.Extension.Helper;

namespace Dapper.Extension.Entities
{
    public class DatabaseTypeInfo
    {
        public String TableSchema { get; }
        public String TableName { get; }
        public Dictionary<String, String> FieldColumnMap { get; }

        public Dictionary<String, String> FieldPrimaryKeyMap { get; }


        //----------------------------------------------------------------//

        public DatabaseTypeInfo(Type type)
        {
            PropertyInfo[] propertiesInfo = type.GetProperties();

            TableName = (type.GetCustomAttribute(type) as TableNameAttribute).TableName;
            FieldColumnMap = TypeMapperHelper.GetFieldColumnMap(propertiesInfo);
            FieldPrimaryKeyMap = TypeMapperHelper.GetPrimaryKeys(propertiesInfo).ToDictionary(p => p.Key, p => p.Value);
            TableSchema = (type.GetCustomAttribute(type) as TableSchemaAttribute)?.SchemaName;
        }

        //----------------------------------------------------------------//

    }
}
