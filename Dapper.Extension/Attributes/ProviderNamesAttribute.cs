using System;

namespace Dapper.Extension.Attributes
{
    public class ProviderNamesAttribute : Attribute
    {
        public String[] ProviderNames { get; }

        //----------------------------------------------------------------//
            
        public ProviderNamesAttribute(params String[] providerNames)
        {
            ProviderNames = providerNames;
        }

        //----------------------------------------------------------------//

    }
}
