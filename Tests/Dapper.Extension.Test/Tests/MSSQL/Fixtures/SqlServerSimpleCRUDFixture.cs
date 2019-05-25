using Dapper.Extension.Enums;
using Dapper.Extension.Test.Tests.Base.Fixtures;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Dapper.Extension.Test.MSSQL.Fixtures
{
    [TestFixture]
    public class SqlServerSimpleCRUDFixture : BaseCRUDFixture
    {

        //----------------------------------------------------------------//

        public const String TEST_CONNECTION_TO_SQL_SERVER = "TestSqlServerConnection";
        public const String TEST_DATABASE = "test_sql_server_database";

        //----------------------------------------------------------------//

        private SqlServerSimpleCRUDFixture()
            : base(SqlProvider.SqlClient)
        {}

        //----------------------------------------------------------------//

        public override Task<String> GetSetupSchemaBaseQuery
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //----------------------------------------------------------------//

        public override Task<String> GetSetupTestDataBaseQuery
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //----------------------------------------------------------------//

        public override Task<string> GetClearTestDataBaseQuery => throw new NotImplementedException();


        //----------------------------------------------------------------//

        public override string GetTestDatabaseName => TEST_DATABASE;

        public override string GetConnectionStringKey => TEST_CONNECTION_TO_SQL_SERVER;

        public override DbConnection CreateDbConnection => new SqlConnection(GetTestDefaultConnectionStringToServer);

        //----------------------------------------------------------------//

    }
}
