using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryCompilerService
{
    IEnumerable<SqlResult> Compile(IReadOnlyCollection<QueryDataModel> models, string providerName);

    int GetParametersLimit(string providerName);
}
