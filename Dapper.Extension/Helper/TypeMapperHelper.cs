using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Dapper.Extension.Attributes;

namespace Dapper.Extension.Helper
{
    public static class TypeMapperHelper
    {

        //----------------------------------------------------------------//

        public static Dictionary<String, String> GetFieldColumnMap(PropertyInfo[] propertiesInfo)
        {
            Type columnAttr = typeof(ColumnAttribute);
            return propertiesInfo.ToDictionary(p => p.Name, p => (p.GetCustomAttribute(columnAttr) as ColumnAttribute)?.ColumnName ?? p.Name);
        }

        //----------------------------------------------------------------//

        public static IEnumerable<KeyValuePair<String, String>> GetPrimaryKeys(PropertyInfo[] propertiesInfo)
        {
            Type primaryKeyAttr = typeof(PrimaryKeyAttribute);
            return propertiesInfo.ToDictionary(p => p.Name, p => (p.GetCustomAttribute(primaryKeyAttr) as PrimaryKeyAttribute)?.PrimaryKeyName)
                                  .Where(p => p.Equals(default(KeyValuePair<String, String>)));
        }

        //----------------------------------------------------------------//

    }
}
