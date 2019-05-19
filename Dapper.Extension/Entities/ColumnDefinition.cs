using System;

namespace Dapper.Extension.Entities
{
    internal class ColumnDefinition
    {

        //----------------------------------------------------------------//

        public String ColumnName { get; }

        public Boolean IsNeedUseDefaultValue { get; }

        public String PropertyName { get; }

        //----------------------------------------------------------------//

        public ColumnDefinition(String columnName, String propertyName, Boolean isNeedUseDefaultValue = false)
        {
            ColumnName = columnName;
            IsNeedUseDefaultValue = isNeedUseDefaultValue;
            PropertyName = propertyName;
        }

        //----------------------------------------------------------------//

    }
}
