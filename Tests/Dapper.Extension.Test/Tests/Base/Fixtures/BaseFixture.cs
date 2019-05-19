using Dapper.Extension.Abstract;
using Dapper.Extension.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace Dapper.Extension.Test.Tests.Base.Fixtures
{
    public abstract class BaseFixture
    {
        //----------------------------------------------------------------//

        public DatabaseAccessor DatabaseAccessor { get; }

        public ISession Session { get; private set; }

        public SqlProvider SqlProvider { get; }

        public IConfiguration Configuration { get; private set; }

        //----------------------------------------------------------------//

        public abstract Task<String> GetSetupBaseQuery { get; }

        public abstract Task<String> GetTearDownBaseQuery { get; }

        public abstract String GetTestDefaultConnectionToServer { get; }

        public abstract String GetTestDatabaseName { get; }

        //----------------------------------------------------------------//

        public const String TEST_NAMESPACE = "Dapper.Extension.Test.TestEntities";
        public const String TEST_ASSEMBLY = "Dapper.Extension.Test";

        //----------------------------------------------------------------//

        public BaseFixture(SqlProvider provider, DbProviderFactory providerFactory)
        {
            SqlProvider = provider;
            var providerFactories = new Dictionary<SqlProvider, DbProviderFactory>{ { provider, providerFactory } };

            DatabaseAccessor = new DatabaseAccessor(providerFactories, TEST_NAMESPACE, TEST_ASSEMBLY);
            Configuration = BuildConfiguration();
        }

        //----------------------------------------------------------------//

        [OneTimeSetUp]
        public async virtual Task BaseFixtureSetUp()
        {
            try
            {
                Session = await DatabaseAccessor.OpenSessionAsync(GetTestDefaultConnectionToServer, SqlProvider);
                Task t_databaseCreateQuery = Session.Connection.ExecuteAsync($"CREATE DATABASE {GetTestDatabaseName};");

                String connectionToTestDatabase = BuildConnectionString(
                    GetTestDefaultConnectionToServer,
                    new Dictionary<String, String> { { nameof(ConnectionStringAttribute.Database), GetTestDatabaseName } });

                await t_databaseCreateQuery;
                await Session.Connection.ExecuteAsync(await GetSetupBaseQuery);
            }
            catch(Exception ex)
            {
                String message = ex.Message;
            }
        }

        //----------------------------------------------------------------//

        [OneTimeTearDown]
        public async virtual Task BaseFixtureTearDown()
        {
            await Session.Connection.ExecuteAsync(await GetTearDownBaseQuery, SqlProvider);
            Session.Dispose();
        }

        //----------------------------------------------------------------//

        private IConfiguration BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        //----------------------------------------------------------------//

        private String BuildConnectionString(
            String baseConnectionString, 
            Dictionary<String, String> parametersLookup)
        {
            DbConnectionStringBuilder connStringBuilder = new DbConnectionStringBuilder();
            connStringBuilder.ConnectionString = baseConnectionString;

            foreach(var pair in parametersLookup)
            {
                connStringBuilder.Add(pair.Key, pair.Value);
            }

            return connStringBuilder.ConnectionString;
        }

        //----------------------------------------------------------------//


    }
}
