using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IDbContextExecutorService
{
    int SaveChangesOptimized(DbContext context, QueryExecutionConfiguration? configuration);

    Task<int> SaveChangesOptimizedAsync(DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken);
}
