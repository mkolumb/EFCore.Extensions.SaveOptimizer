using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryCompilerService
{
    IEnumerable<SqlCommandModel> Compile(IReadOnlyCollection<QueryDataModel> models, string providerName);
}
