﻿using System;

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
            String primaryKeyConstraint = null, 
            Boolean isNeedUseDefaultValue = false)
            : base(columnName, isNeedUseDefaultValue)
        {
            PrimaryKeyConstraintName = primaryKeyConstraint;
        }

        //----------------------------------------------------------------//
    }
}
