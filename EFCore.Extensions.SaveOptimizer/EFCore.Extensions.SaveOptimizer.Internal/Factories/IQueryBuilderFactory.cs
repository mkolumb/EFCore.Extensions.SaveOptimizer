using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Factories;

public interface IQueryBuilderFactory
{
    IQueryBuilder Query(QueryBuilderConfiguration? configuration, string providerName);
}
