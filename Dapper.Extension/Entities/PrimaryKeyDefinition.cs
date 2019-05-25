using System;

namespace Dapper.Extension.Entities
{
    internal class PrimaryKeyColumnDefinition : ColumnDefinition
    {

        //----------------------------------------------------------------//

        public String PrimaryKeyConstraintName { get; }

        //----------------------------------------------------------------//

        public PrimaryKeyColumnDefinition(
            String columnName, 
            String propertyName,
            Boolean isNeedUseDefaultValue = true,
            String primaryKeyConstraint = null)
            : base(columnName, propertyName, isNeedUseDefaultValue)
        {
            PrimaryKeyConstraintName = primaryKeyConstraint;
        }

        //----------------------------------------------------------------//

    }
}
