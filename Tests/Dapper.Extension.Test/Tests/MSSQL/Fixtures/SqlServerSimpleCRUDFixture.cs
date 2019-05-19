using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapper.Extension.Abstract;
using Dapper.Extension.Enums;
using Dapper.Extension.Test.TestEntities;
using Dapper.Extension.Test.Tests.Base.Fixtures;
using NUnit;
using NUnit.Framework;

namespace Dapper.Extension.Test.MSSQL.Fixtures
{
    [TestFixture]
    public class SqlServerSimpleCRUDFixture : BaseCRUDFixture
    {

        //----------------------------------------------------------------//

        private SqlServerSimpleCRUDFixture()
            : base(SqlProvider.SqlClient, DbProviderFactories.GetFactory(nameof(SqlProvider.SqlClient)))
        {}

        public override Task<String> GetSetupBaseQuery
        {
            get
            {
                return File.ReadAllTextAsync($"{Environment.NewLine}/SetupQueries/Postgres/CreateCountryTestDatabase.sql");
            }
        }

        //----------------------------------------------------------------//


        public override string GetTestDefaultConnectionToServer => throw new NotImplementedException();

        public override string GetTestDatabaseName => throw new NotImplementedException();

        //----------------------------------------------------------------//

    }
}
