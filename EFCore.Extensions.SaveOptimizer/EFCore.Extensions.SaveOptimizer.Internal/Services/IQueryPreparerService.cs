using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryPreparerService
{
    void Init(DbContext context);

    IEnumerable<SqlCommandModel> Prepare(DbContext context, int batchSize = InternalConstants.DefaultBatchSize);
}
