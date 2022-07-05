using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryExecutionConfiguratorService : IQueryExecutionConfiguratorService
{
    private static readonly IReadOnlyDictionary<string, QueryBuilderType> QueryBuilders =
        new Dictionary<string, QueryBuilderType>
        {
            { "SqlServer", QueryBuilderType.SqlServer },
            { "Firebird", QueryBuilderType.Firebird },
            { "MySql", QueryBuilderType.MySql },
            { "Maria", QueryBuilderType.MySql },
            { "Oracle", QueryBuilderType.Oracle },
            { "Postgre", QueryBuilderType.Postgres },
            { "SqLite", QueryBuilderType.SqLite },
            { "InMemory", QueryBuilderType.SqLite }
        };

    public QueryExecutionConfiguration Get(string providerName, QueryExecutionConfiguration? configuration)
    {
        QueryExecutionConfiguration config = new();

        if (configuration != null)
        {
            config.BatchSize = configuration.BatchSize;
            config.InsertBatchSize = configuration.InsertBatchSize;
            config.UpdateBatchSize = configuration.UpdateBatchSize;
            config.DeleteBatchSize = configuration.DeleteBatchSize;
            config.ParametersLimit = configuration.ParametersLimit;
        }

        config.BatchSize ??= InternalConstants.DefaultBatchSize;

        config.InsertBatchSize ??= GetMaxBatchSize(providerName, EntityState.Added);

        config.UpdateBatchSize ??= GetMaxBatchSize(providerName, EntityState.Modified);

        config.DeleteBatchSize ??= GetMaxBatchSize(providerName, EntityState.Deleted);

        config.ParametersLimit ??= GetParametersLimit(providerName);

        config.ConcurrencyTokenBehavior ??= ConcurrencyTokenBehavior.ThrowException;

        config.AutoTransactionEnabled ??= true;

        config.AutoTransactionIsolationLevel ??= IsolationLevel.Serializable;

        config.BuilderConfiguration ??= GetBuilderConfiguration(providerName);

        return config;
    }

    private static QueryBuilderConfiguration GetBuilderConfiguration(string providerName)
    {
        QueryBuilderConfiguration configuration = new() { QueryBuilderType = GetBuilderType(providerName) };

        if (providerName.Contains("Firebird"))
        {
            configuration.OptimizeParameters = false;
        }

        return configuration;
    }

    private static QueryBuilderType GetBuilderType(string providerName)
    {
        try
        {
            var key = QueryBuilders.Keys
                .SingleOrDefault(x => providerName.Contains(x, StringComparison.InvariantCultureIgnoreCase));

            return QueryBuilders[key ?? throw new InvalidOperationException()];
        }
        catch
        {
            throw new ArgumentException("Unexpected provider", nameof(providerName));
        }
    }

    private static int? GetMaxBatchSize(string providerName, EntityState state) =>
        state switch
        {
            EntityState.Added when providerName.Contains("Oracle") => InternalConstants.DefaultOracleInsertBatchSize,
            EntityState.Added when providerName.Contains("Firebird") => InternalConstants.DefaultFirebirdInsertBatchSize,
            _ => null
        };

    private static int GetParametersLimit(string providerName)
    {
        if (providerName.Contains("SqlServer"))
        {
            return InternalConstants.DefaultSqlServerParametersLimit;
        }

        if (providerName.Contains("Firebird"))
        {
            return InternalConstants.DefaultFirebirdParametersLimit;
        }

        if (providerName.Contains("Postgre"))
        {
            return InternalConstants.DefaultPostgresParametersLimit;
        }

        if (providerName.Contains("Sqlite") || providerName.Contains("InMemory"))
        {
            return InternalConstants.DefaultSqLiteParametersLimit;
        }

        return InternalConstants.DefaultParametersLimit;
    }
}
