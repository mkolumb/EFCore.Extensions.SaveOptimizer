using EFCore.Extensions.SaveOptimizer.Models;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Services;

public interface IQueryCompilerService
{
    IEnumerable<SqlResult> Compile(IReadOnlyCollection<QueryDataModel> models);
}
