using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;

namespace EFCore.Extensions.SaveOptimizer.Internal.Factories;

public interface IQueryBuilderFactory
{
    IQueryBuilder Query(string providerName);
}
