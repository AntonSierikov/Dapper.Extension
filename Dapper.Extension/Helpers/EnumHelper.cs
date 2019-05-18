using Dapper.Extension.Attributes;
using Dapper.Extension.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace Dapper.Extension.Helpers
{
    public static class EnumHelper
    {

        //----------------------------------------------------------------//

        public static SqlProvider? GetProviderByName(String providerName)
        {
            FieldInfo[] fields = typeof(SqlProvider).GetFields();

            foreach(FieldInfo field in fields)
            {
                ProviderNamesAttribute providerNamesAttr = field.GetCustomAttribute<ProviderNamesAttribute>();
                if (providerNamesAttr.ProviderNames.Contains(providerName))
                {
                    return (SqlProvider)field.GetValue(null); 
                }
            }

            return null;
        }

        //----------------------------------------------------------------//

    }
}
