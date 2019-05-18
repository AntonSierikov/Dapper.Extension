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

        //----------------------------------------------------------------//

        public DatabaseAccessor(IDictionary<SqlProvider, DbProviderFactory> providerFactories)
        {
            ProviderFactories = providerFactories;
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
            String @namespace)
        {
            InitCustomMapTypes(@namespace);
        }

        //----------------------------------------------------------------//

        #region Public Methods

        public async Task<ISession> OpenSession(String connectionString, SqlProvider provider, IsolationLevel? isolationLevel)
        {
            DbProviderFactory providerFactory = ProviderFactories[SqlProvider.NpSql];
            Session session = new Session(this, SqlProvider.NpSql);

            await session.OpenConnection(connectionString);

            if (isolationLevel.HasValue)
            {
                session.OpenTransaction(isolationLevel.Value);
            }

            return session;
        }

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

        private void InitCustomMapTypes(String @namespace)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> databaseTypes = executingAssembly.GetTypes().Where(t => t.IsClass && t.Namespace.Equals(@namespace));
            InitCustomMapTypes(databaseTypes);
        }


        //----------------------------------------------------------------//

        private void InitSqlGenerators()
        {
            SqlGenerators.Add(SqlProvider.NpSql, new NpgSqlGenerator());
            SqlGenerators.Add(SqlProvider.SqlClient, new SqlClientGenerator());
        }

        //----------------------------------------------------------------//

        #endregion
    }
}
