using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryExecutorService
{
    int Execute(DbContext context,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout);

    Task<int> ExecuteAsync(DbContext context,
        IDbContextTransaction transaction,
        ISqlCommandModel sql,
        int? timeout,
        CancellationToken cancellationToken);
}
