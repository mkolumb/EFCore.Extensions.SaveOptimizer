using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Factories;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    public IQueryBuilder Query(string providerName)
    {
        if (providerName.Contains("SqlServer"))
        {
            return new SqlServerQueryBuilder();
        }

        if (providerName.Contains("Firebird"))
        {
            return new FirebirdQueryBuilder();
        }

        if (providerName.Contains("MySql"))
        {
            return new MySqlQueryBuilder();
        }

        if (providerName.Contains("Maria"))
        {
            return new MySqlQueryBuilder();
        }

        if (providerName.Contains("Oracle"))
        {
            return new OracleAllQueryBuilder();
        }

        if (providerName.Contains("Postgre"))
        {
            return new PostgresQueryBuilder();
        }

        if (providerName.Contains("Sqlite"))
        {
            return new SqliteQueryBuilder();
        }

        if (providerName.Contains("InMemory"))
        {
            return new SqliteQueryBuilder();
        }

        throw new ArgumentException("Unexpected provider", nameof(providerName));
    }
}
