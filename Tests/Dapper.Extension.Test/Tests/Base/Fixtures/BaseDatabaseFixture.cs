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
    public abstract class BaseDatabaseFixture
    {
        //----------------------------------------------------------------//

        public DatabaseAccessor DatabaseAccessor { get; }

        public ISession Session { get; private set; }

        public SqlProvider SqlProvider { get; }

        public IConfiguration Configuration { get; private set; }

        //----------------------------------------------------------------//

        public abstract Task<String> GetSetupBaseQuery { get; }

        public abstract String GetTestDefaultConnectionToServer { get; }

        public abstract String GetTestDatabaseName { get; }

        //----------------------------------------------------------------//

        public const String TEST_NAMESPACE = "Dapper.Extension.Test.TestEntities";
        public const String TEST_ASSEMBLY = "Dapper.Extension.Test";

        //----------------------------------------------------------------//

        public BaseDatabaseFixture(SqlProvider provider, DbProviderFactory providerFactory)
        {
            SqlProvider = provider;
            var providerFactories = new Dictionary<SqlProvider, DbProviderFactory>{ { provider, providerFactory } };

            DatabaseAccessor = new DatabaseAccessor(providerFactories, TEST_NAMESPACE, TEST_ASSEMBLY);
            Configuration = BuildConfiguration();
        }

        //----------------------------------------------------------------//

        [OneTimeSetUp]
        public async virtual Task BaseFixtureSetUpAsync()
        {
            try
            {
                Task<String> t_getSetupBaseQuery = GetSetupBaseQuery;

                await CreateTestDatabaseAsync();
                var additionalParams = new Dictionary<String, String>(){{ nameof(ConnectionStringAttribute.Database), GetTestDatabaseName }};

                String connectionToTestDatabase = BuildConnectionString(GetTestDefaultConnectionToServer, additionalParams);
                Session = await DatabaseAccessor.OpenSessionAsync(connectionToTestDatabase, SqlProvider);

                String baseSetupSchemaQuery = await t_getSetupBaseQuery;
                Int32 resultSchemaSetup = await Session.Connection.ExecuteAsync(baseSetupSchemaQuery);
            }
            catch(Exception ex)
            {
                String message = ex.Message;
            }
        }

        //----------------------------------------------------------------//

        [OneTimeTearDown]
        public async virtual Task BaseFixtureTearDownAsync()
        {
            try
            {
                Session.Dispose();
                await ClearUpAsync();
            }
            catch(Exception ex)
            {
                String message = ex.Message;
            }
        }

        //----------------------------------------------------------------//

        private async Task CreateTestDatabaseAsync()
        {
            using (Session = await DatabaseAccessor.OpenSessionAsync(GetTestDefaultConnectionToServer, SqlProvider))
            {
                Int32 resultDatabaseCreate = await Session.Connection.ExecuteAsync($"CREATE DATABASE {GetTestDatabaseName};");
            }
        }

        //----------------------------------------------------------------//

        private async Task ClearUpAsync()
        {
            using (Session = await DatabaseAccessor.OpenSessionAsync(GetTestDefaultConnectionToServer, SqlProvider))
            {
                var param = new { TestDatabase = GetTestDatabaseName };

                String restrictConnectionQuery = $"UPDATE pg_database SET datallowconn = 'false' WHERE datname = @{nameof(param.TestDatabase)}";
                String closeCurrentSessionsQuery = $@"SELECT pg_terminate_backend(pg_stat_activity.pid)
                                                 FROM pg_stat_activity
                                                 WHERE pg_stat_activity.datname = @{nameof(param.TestDatabase)} AND pid<> pg_backend_pid();";
                String dropDatabaseQuery = $"DROP Database {GetTestDatabaseName}";
                
                Int32 resultRestrictConnection = await Session.Connection.ExecuteAsync(restrictConnectionQuery, param);
                Int32 closeCurrentSessions = await Session.Connection.ExecuteAsync(closeCurrentSessionsQuery, param);
                Int32 resultDatabaseDrop = await Session.Connection.ExecuteAsync(dropDatabaseQuery);
            }
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
