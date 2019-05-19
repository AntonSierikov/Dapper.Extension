using Dapper.Extension.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.Extension.Helpers
{
    public class OperatorHelper
    {
        internal static String OperatorWithSpaces(String @operator) => $" {@operator} ";

        //----------------------------------------------------------------//

        internal static String JoinPropertyColumn<TEntity>(
            IDictionary<String, TEntity> propertyColumns, 
            String propertyColumnsOperator, 
            String termOperator)
            where TEntity: ColumnDefinition
        {
            String propertyColumnsOperatorWithSpaces = OperatorWithSpaces(propertyColumnsOperator);
            String termOperatorWithSpaces = OperatorWithSpaces(termOperator);
            return String.Join(termOperatorWithSpaces, propertyColumns
                .Select(pc => $"{pc.Value.ColumnName} {propertyColumnsOperatorWithSpaces} @{(pc.Value.PropertyName)}"));
        }

        //----------------------------------------------------------------//
    } 
}
