using System;

namespace Dapper.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PrimaryKeyAttribute: Attribute
    {
        public String PrimaryKeyName { get; }

        //----------------------------------------------------------------//

        public PrimaryKeyAttribute(String primaryKey)
        {
            PrimaryKeyName = primaryKey;
        }

        //----------------------------------------------------------------//

    }
}
