using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IDbContextExecutorService
{
    IExecutionResultModel SaveChangesOptimized(DbContext context, QueryExecutionConfiguration? configuration);

    Task<IExecutionResultModel> SaveChangesOptimizedAsync(DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken);
}
