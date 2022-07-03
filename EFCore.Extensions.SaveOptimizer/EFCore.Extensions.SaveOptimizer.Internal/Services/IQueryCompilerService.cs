using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryCompilerService
{
    IEnumerable<ISqlCommandModel> Compile(IReadOnlyCollection<QueryDataModel> models, string providerName, QueryBuilderConfiguration? configuration);
}
