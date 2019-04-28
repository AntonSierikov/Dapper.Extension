using Dapper.Extension.Entities;
using System;
using System.Collections.Generic;

namespace Dapper.Extension.Providers
{
    internal static class DatabaseEntitiesInfoProvider
    {
        private static readonly Dictionary<Int32, DatabaseTypeInfo> _databaseEntitiesInfo;


        //----------------------------------------------------------------//

        static DatabaseEntitiesInfoProvider()
        {
            _databaseEntitiesInfo = new Dictionary<Int32, DatabaseTypeInfo>();
        }

        //----------------------------------------------------------------//

        public static void AddDatabaseEntityInfo(Type type)
        {
            _databaseEntitiesInfo[type.GetHashCode()] = new DatabaseTypeInfo(type);
        }

        //----------------------------------------------------------------//

        public static DatabaseTypeInfo GetDatabaseEntityInfo<T>()
        {
            return _databaseEntitiesInfo[typeof(T).GetHashCode()];
        }

        //----------------------------------------------------------------//

    }
}
