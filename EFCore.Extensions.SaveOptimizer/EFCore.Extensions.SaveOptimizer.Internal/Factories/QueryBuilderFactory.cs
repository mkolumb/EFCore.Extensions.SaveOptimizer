using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Factories;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    public IQueryBuilder Query(QueryBuilderConfiguration? configuration) =>
        configuration?.QueryBuilderType switch
        {
            QueryBuilderType.SqlServer => new SqlServerQueryBuilder(configuration),
            QueryBuilderType.SqLite => new SqliteQueryBuilder(configuration),
            QueryBuilderType.Oracle => new OracleQueryBuilder(configuration),
            QueryBuilderType.Firebird => new FirebirdQueryBuilder(configuration),
            QueryBuilderType.MySql => new MySqlQueryBuilder(configuration),
            QueryBuilderType.Postgres => new PostgresQueryBuilder(configuration),
            _ => throw new ArgumentException("Unexpected provider", nameof(configuration))
        };
}
