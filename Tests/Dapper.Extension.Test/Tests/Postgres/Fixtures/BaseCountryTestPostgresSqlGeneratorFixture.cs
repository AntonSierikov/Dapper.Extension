using Dapper.Extension.SqlGenerators;
using Dapper.Extension.Test.Tests.Base.Fixtures;
using System;

namespace Dapper.Extension.Test.Tests.Postgres.Fixtures
{
    internal class BaseCountryTestPostgresSqlGeneratorFixture : BaseCountryTestSQLGeneratorFixture
    {
        public BaseCountryTestPostgresSqlGeneratorFixture()
            : base(new NpgSqlGenerator(), "country")
        {}
    }
}
