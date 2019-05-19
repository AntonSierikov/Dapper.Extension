using Dapper.Extension.Abstract;
using Dapper.Extension.Entities;
using Dapper.Extension.Enums;
using Dapper.Extension.Sessions;
using Dapper.Extension.SqlGenerators;
using Dapper.Extension.TypeMappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Dapper.Extension.Test")]

namespace Dapper.Extension
{
    public class DatabaseAccessor
    { 
        //----------------------------------------------------------------//

        public IDictionary<SqlProvider, DbProviderFactory> ProviderFactories { get; }

        //----------------------------------------------------------------//

        internal Dictionary<Int32, DatabaseTypeInfo> DatabaseEntitiesInfo { get; private set; }

        internal Dictionary<SqlProvider, BaseSqlGenerator> SqlGenerators { get; private set; }

        internal Dictionary<Int32, Object> CustomCommands { get; private set; }

        internal Dictionary<Int32, Object> CustomQueries { get; private set; }

        //----------------------------------------------------------------//

        private DatabaseAccessor(IDictionary<SqlProvider, DbProviderFactory> providerFactories)
        {
            ProviderFactories = providerFactories;
            DatabaseEntitiesInfo = new Dictionary<Int32, DatabaseTypeInfo>();
            SqlGenerators = new Dictionary<SqlProvider, BaseSqlGenerator>();
            CustomCommands = new Dictionary<Int32, Object>();
            CustomQueries = new Dictionary<Int32, Object>();
            InitSqlGenerators();
        }

        //----------------------------------------------------------------//

        public DatabaseAccessor(
            IDictionary<SqlProvider, DbProviderFactory> providerFactories,
            IEnumerable<Type> types)
            : this(providerFactories)
        {
            InitCustomMapTypes(types);
        }

        //----------------------------------------------------------------//

        public DatabaseAccessor(
            IDictionary<SqlProvider, DbProviderFactory> providerFactories, 
            String @namespace,
            String assemblyName)
            : this(providerFactories)
        {
            InitCustomMapTypes(@namespace, assemblyName);
        }

        //----------------------------------------------------------------//

        #region Public Methods

        public async Task<ISession> OpenSessionAsync(
            String connectionString, 
            SqlProvider provider,
            IsolationLevel? isolationLevel = null)
        {
            DbProviderFactory providerFactory = ProviderFactories[SqlProvider.NpgSql];
            Session session = new Session(this, SqlProvider.NpgSql);

            await session.OpenConnection(connectionString);

            if (isolationLevel.HasValue)
            {
                session.OpenTransaction(isolationLevel.Value);
            }

            return session;
        }

        //----------------------------------------------------------------//

        public void AddCustomCommand<TEntity, TCustomCommand>(TCustomCommand customCommand)
        {
            CustomCommands[typeof(TEntity).GetHashCode()] = customCommand;
        }

        //----------------------------------------------------------------//

        public void AddCustomQuery<TEntity, TCustomQuery>(TCustomQuery customQuery)
        {
            CustomQueries[typeof(TEntity).GetHashCode()] = customQuery;
        }

        //----------------------------------------------------------------//


        #endregion

        #region Private methods

        //----------------------------------------------------------------//

        private void InitCustomMapTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                SqlMapper.SetTypeMap(type, new TypeMapper(type));
                DatabaseEntitiesInfo[type.GetHashCode()] = new DatabaseTypeInfo(type);
            }
        }

        //----------------------------------------------------------------//

        private void InitCustomMapTypes(String @namespace, String assemblyName)
        {
            Assembly executingAssembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name.Equals(assemblyName));
            IEnumerable<Type> databaseTypes = executingAssembly.GetTypes().Where(t => t.IsClass && String.Equals(t.Namespace, @namespace));
            InitCustomMapTypes(databaseTypes);
        }

        //----------------------------------------------------------------//

        private void InitSqlGenerators()
        {
            SqlGenerators.Add(SqlProvider.NpgSql, new NpgSqlGenerator());
            SqlGenerators.Add(SqlProvider.SqlClient, new SqlClientGenerator());
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
