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

        return config;
    }

    private static int? GetMaxBatchSize(string providerName, EntityState state)
    {
        if (state == EntityState.Added)
        {
            if (providerName.Contains("Firebird"))
            {
                return 1;
            }

            if (providerName.Contains("Oracle"))
            {
                return 20;
            }
        }

        return null;
    }

    private static int GetParametersLimit(string providerName)
    {
        if (providerName.Contains("SqlServer"))
        {
            return 1024;
        }

        if (providerName.Contains("Postgre"))
        {
            return 31768;
        }

        if (providerName.Contains("Sqlite") || providerName.Contains("InMemory"))
        {
            return 512;
        }

        return InternalConstants.DefaultParametersLimit;
    }
}
