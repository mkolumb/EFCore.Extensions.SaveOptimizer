using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryPreparerService
{
    void Init(DbContext context);

    IEnumerable<SqlResult> Prepare(DbContext context, int batchSize = InternalConstants.DefaultBatchSize);
}
