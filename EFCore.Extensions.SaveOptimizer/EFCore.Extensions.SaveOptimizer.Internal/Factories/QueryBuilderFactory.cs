using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Factories;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    public IQueryBuilder Query(QueryBuilderConfiguration? configuration, string providerName)
    {
        if (providerName.Contains("SqlServer"))
        {
            return new SqlServerQueryBuilder(configuration);
        }

        if (providerName.Contains("Firebird"))
        {
            return new FirebirdQueryBuilder(configuration);
        }

        if (providerName.Contains("MySql"))
        {
            return new MySqlQueryBuilder(configuration);
        }

        if (providerName.Contains("Maria"))
        {
            return new MySqlQueryBuilder(configuration);
        }

        if (providerName.Contains("Oracle"))
        {
            return new OracleAllQueryBuilder(configuration);
        }

        if (providerName.Contains("Postgre"))
        {
            return new PostgresQueryBuilder(configuration);
        }

        if (providerName.Contains("Sqlite"))
        {
            return new SqliteQueryBuilder(configuration);
        }

        if (providerName.Contains("InMemory"))
        {
            return new SqliteQueryBuilder(configuration);
        }

        throw new ArgumentException("Unexpected provider", nameof(providerName));
    }
}
