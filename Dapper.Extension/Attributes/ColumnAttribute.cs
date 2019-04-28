using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    class ColumnAttribute : Attribute
    {
        public String ColumnName { get;  }

        //----------------------------------------------------------------/r/

        public ColumnAttribute (String columnName)
        {
            ColumnName = columnName;
        }

        //----------------------------------------------------------------//

    }
}
