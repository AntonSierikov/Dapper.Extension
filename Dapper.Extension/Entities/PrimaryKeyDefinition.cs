using System;

namespace Dapper.Extension.Entities
{
    internal class PrimaryKeyDefinition : ColumnDefinition
    {

        //----------------------------------------------------------------//

        public String PrimaryKeyConstraintName { get; }

        //----------------------------------------------------------------//

        public PrimaryKeyDefinition(
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
