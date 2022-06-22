using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryPreparerService
{
    IEnumerable<SqlResult> Prepare(DbContext context);
}
