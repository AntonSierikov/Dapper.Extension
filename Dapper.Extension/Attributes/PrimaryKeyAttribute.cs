using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PrimaryKeyAttribute: ColumnAttribute
    {

        //----------------------------------------------------------------//

        public String PrimaryKeyConstraintName { get; }

        //----------------------------------------------------------------//

        public PrimaryKeyAttribute(
            String columnName,
            Boolean isNeedUseDefaultValue = false,
            String primaryKeyConstraint = null)
            : base(columnName, isNeedUseDefaultValue)
        {
            PrimaryKeyConstraintName = primaryKeyConstraint;
        }

        //----------------------------------------------------------------//
    }
}
