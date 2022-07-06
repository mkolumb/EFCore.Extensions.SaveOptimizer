using EFCore.Extensions.SaveOptimizer.Internal.Configuration;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryExecutionConfiguratorService
{
    QueryExecutionConfiguration Get(string providerName, QueryExecutionConfiguration? configuration);
}
