using Dapper.Extension.Enums;
using Dapper.Extension.Test.Tests.Base.Fixtures;
using Microsoft.Extensions.Configuration;
using Moq;
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
        private String _defaultConnectionString;

        //----------------------------------------------------------------//

        public const String TEST_CONNECTION_TO_POSTGRES = "TestPostgresConnection";

        public const String TEST_DATABASE = "test_postgre_database";

        //----------------------------------------------------------------//

        public PostgresSimpleCRUDFixture()
            : base(SqlProvider.NpgSql, MoqProviderFactory())
        {}

        //----------------------------------------------------------------//

        public override Task<String> GetSetupBaseQuery
        {
            get
            {
                return File.ReadAllTextAsync($@"{Environment.CurrentDirectory}/SetupQueries/Postgres/CreateCountrySchemaToTestDatabase.sql");
            }
        }

        //----------------------------------------------------------------//

        public override string GetTestDefaultConnectionToServer
        {
            get
            {
                if(_defaultConnectionString == null)
                {
                    _defaultConnectionString = Configuration.GetConnectionString(TEST_CONNECTION_TO_POSTGRES);
                }

                return _defaultConnectionString;
            }
        }

        //----------------------------------------------------------------//

        public override string GetTestDatabaseName => TEST_DATABASE;

        //----------------------------------------------------------------//

        //need to search better method
        private static DbProviderFactory MoqProviderFactory()
        {
            Mock<DbProviderFactory> mock = new Mock<DbProviderFactory>();
            mock.Setup(m => m.CreateConnection()).Returns(() => new NpgsqlConnection());
            return mock.Object;
        }

        //----------------------------------------------------------------//

    }
}
