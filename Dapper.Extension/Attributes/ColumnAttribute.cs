using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public String ColumnName { get;  }
        public Boolean IsNeedUseDefaultValue { get; }

        //----------------------------------------------------------------/r/

        public ColumnAttribute (String columnName, Boolean isNeedUseDefaultValue = false)
        {
            ColumnName = columnName;
        }

        //----------------------------------------------------------------//

    }
}
