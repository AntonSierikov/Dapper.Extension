using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.Extension.Helper
{
    public class OperatorHelper
    {
        public static String OperatorWithSpaces(String @operator) => $" {@operator} "; 

        public static String JoinProperyColumn(IDictionary<String, String> propertyColumns, 
                                               String propertyColumnsOperator, 
                                               String termOperator)
        {
            String propertyColumnsOperatorWithSpaces = OperatorWithSpaces(propertyColumnsOperator);
            String termOperatorWithSpaces = OperatorWithSpaces(termOperator);
            return String.Join(termOperatorWithSpaces, propertyColumns.Select(pc => $"{pc.Key} {propertyColumnsOperatorWithSpaces} @{pc.Value}"));
        }
    } 
}
