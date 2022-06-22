using Microsoft.EntityFrameworkCore;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryPreparerService
{
    IEnumerable<SqlResult> Prepare(DbContext context);
}
