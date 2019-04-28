using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public String TableName { get; }

        //----------------------------------------------------------------//

        public TableNameAttribute(String name)
        {
            TableName = name;
        }

        //----------------------------------------------------------------//

    }
}
