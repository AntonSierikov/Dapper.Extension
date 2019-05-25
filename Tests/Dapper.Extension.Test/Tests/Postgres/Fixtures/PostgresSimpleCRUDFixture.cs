using Dapper.Extension.Enums;
using Dapper.Extension.Test.Tests.Base.Fixtures;
using Npgsql;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

namespace Dapper.Extension.Test.Tests.Postgres.Fixtures
{
    [TestFixture]
    class PostgresSimpleCRUDFixture : BaseCRUDFixture
    {
        //----------------------------------------------------------------//

        public const String TEST_CONNECTION_TO_POSTGRES = "TestPostgresConnection";

        public const String TEST_DATABASE = "test_postgre_database";

        //----------------------------------------------------------------//

        public PostgresSimpleCRUDFixture()
            : base(SqlProvider.NpgSql)
        {}

        //----------------------------------------------------------------//

        public override Task<String> GetSetupSchemaBaseQuery
        {
            get
            {
                return File.ReadAllTextAsync($"{Environment.CurrentDirectory}/SetupQueries/Postgres/CreateCountrySchemaToTestDatabase.sql");
            }
        }

        //----------------------------------------------------------------//

        public override Task<string> GetClearTestDataBaseQuery
        {
            get
            {
                return File.ReadAllTextAsync($"{Environment.CurrentDirectory}/SetupQueries/Postgres/ClearUpTestDataOfCountryDatabase.sql");
            }
        }

        //----------------------------------------------------------------//

        public override Task<String> GetSetupTestDataBaseQuery
        {
            get
            {
                return File.ReadAllTextAsync($"{Environment.CurrentDirectory}/SetupQueries/Postgres/InsertTestDataToCountryDatabase.sql");
            }
        }

        //----------------------------------------------------------------//

        public override string GetTestDatabaseName => TEST_DATABASE;

        public override DbConnection CreateDbConnection => new NpgsqlConnection(GetTestDefaultConnectionStringToServer);

        public override string GetConnectionStringKey => TEST_CONNECTION_TO_POSTGRES;

        //----------------------------------------------------------------//
    }
}
