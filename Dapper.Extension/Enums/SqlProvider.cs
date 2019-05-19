using Dapper.Extension.Attributes;

namespace Dapper.Extension.Enums
{
    public enum SqlProvider
    {
        [ProviderNames(nameof(NpgSql))]
        NpgSql,
        [ProviderNames(nameof(SqlClient), "System.Data.SqlClient")]
        SqlClient
    }
}
