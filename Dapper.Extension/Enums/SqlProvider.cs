using Dapper.Extension.Attributes;

namespace Dapper.Extension.Enums
{
    public enum SqlProvider
    {
        [ProviderNames(nameof(NpSql))]
        NpSql,
        [ProviderNames(nameof(SqlClient), "System.Data.SqlClient")]
        SqlClient
    }
}
