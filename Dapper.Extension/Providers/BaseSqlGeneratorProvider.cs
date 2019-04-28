using Dapper.Extension.Enums;
using Dapper.Extension.SqlGenerators;
using System.Collections.Generic;

namespace Dapper.Extension.Providers
{
    internal static class BaseSqlGeneratorProvider
    {
        private static readonly Dictionary<SqlProvider, BaseSqlGenerator> _sqlGeneratorsDictionary = new Dictionary<SqlProvider, BaseSqlGenerator>();

        //----------------------------------------------------------------//

        public static void AddSqlGenerator(SqlProvider provider, BaseSqlGenerator generator)
        {
            if (!_sqlGeneratorsDictionary.ContainsKey(provider))
            {
                _sqlGeneratorsDictionary[provider] = generator;
            }
        }

        //----------------------------------------------------------------//

        public static BaseSqlGenerator GetSqlGenerator(SqlProvider provider)
        {
            return _sqlGeneratorsDictionary[provider];
        }

        //----------------------------------------------------------------//

    }
}
