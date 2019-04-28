using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableSchemaAttribute : Attribute
    {
        public String SchemaName { get; }

        //----------------------------------------------------------------//

        public TableSchemaAttribute(String schemaName)
        {
            SchemaName = schemaName;
        }

        //----------------------------------------------------------------//

    }
}
