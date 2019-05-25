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
using System.Data;
using Moq;

namespace Dapper.Extension.Test.Tests.Base.Fixtures
{
    public abstract class BaseDatabaseFixture
    {
        private String _defaultConnectionString;

        //----------------------------------------------------------------//

        public DatabaseAccessor DatabaseAccessor { get; }

        public ISession Session { get; private set; }

        public SqlProvider SqlProvider { get; }

        public IConfiguration Configuration { get; private set; }

        //----------------------------------------------------------------//

        public abstract String GetTestDatabaseName { get; }

        public abstract String GetConnectionStringKey { get;  }

        public abstract DbConnection CreateDbConnection { get;  }

        public abstract Task<String> GetSetupSchemaBaseQuery { get; }

        public abstract Task<String> GetSetupTestDataBaseQuery { get; }

        public abstract Task<String> GetClearTestDataBaseQuery { get; }

        //----------------------------------------------------------------//

        public const String TEST_NAMESPACE = "Dapper.Extension.Test.TestEntities";
        public const String TEST_ASSEMBLY = "Dapper.Extension.Test";

        //----------------------------------------------------------------//

        public BaseDatabaseFixture(SqlProvider provider)
        {
            SqlProvider = provider;
            var providerFactories = new Dictionary<SqlProvider, DbProviderFactory>{ { provider, MoqProviderFactory() } };

            DatabaseAccessor = new DatabaseAccessor(providerFactories, TEST_NAMESPACE, TEST_ASSEMBLY);
            Configuration = BuildConfiguration();
        }

        //----------------------------------------------------------------//

        protected String GetTestDefaultConnectionStringToServer
        {
            get
            {
                if (_defaultConnectionString == null)
                {
                    _defaultConnectionString = Configuration.GetConnectionString(GetConnectionStringKey);
                }

                return _defaultConnectionString;
            }
        }

        //----------------------------------------------------------------//

        #region Public Methods

        [OneTimeSetUp]
        public async virtual Task BaseFixtureSetUpAsync()
        {
            try
            {
                Task<String> t_getSetupBaseQuery = GetSetupSchemaBaseQuery;
                Session = await DatabaseAccessor.OpenSessionAsync(GetTestDefaultConnectionStringToServer, SqlProvider);

                await CreateTestDatabaseAsync();
                var additionalParams = new Dictionary<String, String>(){{ nameof(ConnectionStringAttribute.Database), GetTestDatabaseName }};

                String connectionToTestDatabase = BuildConnectionString(GetTestDefaultConnectionStringToServer, additionalParams);
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
                //close active connection
                Session.Dispose();

                await ClearUpAsync();
            }
            catch(Exception ex)
            {
                String message = ex.Message;
            }
        }

        //----------------------------------------------------------------//

        [SetUp]
        public async virtual Task SetUpTestData()
        {
            try
            {
                Console.WriteLine("Setup Test Data is started");
                await ExecuteSetupTestData();
                Console.WriteLine("Setup Test Data is finished successfully");
            }
            catch(Exception ex)
            {
                String errorMessage = ex.Message;
                Console.WriteLine("Setup Test Data is fell down");
            }
        }

        //----------------------------------------------------------------//

        [TearDown]
        public async virtual Task ClearUpTestData()
        {
            try
            {
                Console.WriteLine("Clearing Test Data is started");
                await ExecuteClearUpTestData();
                Console.WriteLine("Clearing Test Data is finished successfully");
            }
            catch (Exception ex)
            {
                String errorMessage = ex.Message;
                Console.WriteLine("Clearings Test Data is fell down");
            }
        }

        //----------------------------------------------------------------//


        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(String query, Object parameters)
        {
            return await Session.Connection.QueryAsync<T>(query, parameters);
        }

        #endregion

        //----------------------------------------------------------------//


        #region Private Methods 

        //----------------------------------------------------------------//

        private async Task CreateTestDatabaseAsync()
        {
            Int32 resultDatabaseCreate = await Session.Connection.ExecuteAsync($"CREATE DATABASE {GetTestDatabaseName};");
        }

        //----------------------------------------------------------------//

        private async Task ClearUpAsync()
        {
            using (Session = await DatabaseAccessor.OpenSessionAsync(GetTestDefaultConnectionStringToServer, SqlProvider))
            {
                var param = new { TestDatabase = GetTestDatabaseName };

                String restrictConnectionQuery = $"UPDATE pg_database SET datallowconn = 'false' WHERE datname = @{nameof(param.TestDatabase)};";
                String closeCurrentSessionsQuery = $@"SELECT pg_terminate_backend(pg_stat_activity.pid)
                                                 FROM pg_stat_activity
                                                 WHERE pg_stat_activity.datname = @{nameof(param.TestDatabase)} AND pid<> pg_backend_pid();";
                String dropDatabaseQuery = $"DROP Database {GetTestDatabaseName};";

                Int32 resultRestrictConnection = await Session.Connection.ExecuteAsync(restrictConnectionQuery, param);
                Int32 closeCurrentSessions = await Session.Connection.ExecuteAsync(closeCurrentSessionsQuery, param);
                Int32 resultDatabaseDrop = await Session.Connection.ExecuteAsync(dropDatabaseQuery);
            }
        }

        //----------------------------------------------------------------//

        private async Task ExecuteSetupTestData()
        {
            String setupQuery = await GetSetupTestDataBaseQuery;
            await Session.Connection.ExecuteAsync(setupQuery);
        }

        //----------------------------------------------------------------//

        private async Task ExecuteClearUpTestData()
        {
            String clearDataQuery = await GetClearTestDataBaseQuery;
            await Session.Connection.ExecuteAsync(clearDataQuery);
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

        //need to search better method
        private DbProviderFactory MoqProviderFactory()
        {
            Mock<DbProviderFactory> mock = new Mock<DbProviderFactory>();
            mock.Setup(m => m.CreateConnection()).Returns(() => CreateDbConnection);
            return mock.Object;
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
