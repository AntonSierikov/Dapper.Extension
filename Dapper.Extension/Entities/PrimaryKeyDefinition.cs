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
            Boolean isNeedUseDefaultValue = true,
            String primaryKeyConstraint = null)
            : base(columnName, isNeedUseDefaultValue)
        {
            PrimaryKeyConstraintName = primaryKeyConstraint;
        }

        //----------------------------------------------------------------//

    }
}
