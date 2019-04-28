using Dapper.Extension.Providers;
using Dapper.Extension.TypeMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dapper.Extension.Managers
{
    public static class ConfigurationManager
    {

        //----------------------------------------------------------------//

        public static void InitCustomMapTypes(IEnumerable<Type> types)
        {
            foreach(Type type in types)
            {
                SqlMapper.SetTypeMap(type, new TypeMapper(type));
                DatabaseEntitiesInfoProvider.AddDatabaseEntityInfo(type);
            }
        }

        //----------------------------------------------------------------//

        public static void InitCustomMapTypes(String @namespace)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> databaseTypes = executingAssembly.GetTypes().Where(t => t.IsClass && t.Namespace.Equals(@namespace));
            InitCustomMapTypes(databaseTypes);
        }

        //----------------------------------------------------------------//
    }
}
