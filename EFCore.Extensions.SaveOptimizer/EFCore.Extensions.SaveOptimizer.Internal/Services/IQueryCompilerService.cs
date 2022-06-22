using EFCore.Extensions.SaveOptimizer.Internal.Models;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryCompilerService
{
    IEnumerable<SqlResult> Compile(IReadOnlyCollection<QueryDataModel> models, string providerName);
}
