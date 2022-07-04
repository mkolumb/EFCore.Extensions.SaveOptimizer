using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class QueryExecutionConfiguratorService : IQueryExecutionConfiguratorService
{
    public QueryExecutionConfiguration Get(string providerName, QueryExecutionConfiguration? configuration = null)
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

        config.BuilderConfiguration ??= GetBuilderConfiguration(providerName);

        return config;
    }

    private static QueryBuilderConfiguration GetBuilderConfiguration(string providerName)
    {
        QueryBuilderConfiguration configuration = new();

        if (providerName.Contains("Firebird"))
        {
            configuration.OptimizeParameters = false;
        }

        return configuration;
    }

    private static int? GetMaxBatchSize(string providerName, EntityState state) =>
        state switch
        {
            EntityState.Added when providerName.Contains("Oracle") => InternalConstants.DefaultOracleBatchSize,
            EntityState.Added when providerName.Contains("Firebird") => InternalConstants.DefaultFirebirdBatchSize,
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
