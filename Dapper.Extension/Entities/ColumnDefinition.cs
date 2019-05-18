using System;

namespace Dapper.Extension.Entities
{
    internal class ColumnDefinition
    {

        //----------------------------------------------------------------//

        public String ColumnName { get; }

        public Boolean IsNeedUseDefaultValue { get; }

        //----------------------------------------------------------------//

        public ColumnDefinition(String columnName, Boolean isNeedUseDefaultValue = false)
        {
            ColumnName = columnName;
            IsNeedUseDefaultValue = isNeedUseDefaultValue;
        }

        //----------------------------------------------------------------//

    }
}
