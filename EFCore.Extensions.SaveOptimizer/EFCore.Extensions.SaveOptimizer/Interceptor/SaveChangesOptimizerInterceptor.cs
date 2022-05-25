using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.Extensions.SaveOptimizer.Interceptor;

public class SaveChangesOptimizerInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) =>
        result;

    public int SavedChanges(SaveChangesCompletedEventData eventData, int result) => result;

    public void SaveChangesFailed(DbContextErrorEventData eventData)
    {
    }

    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new()) =>
        result;

    public async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new()) =>
        result;

    public async Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
        CancellationToken cancellationToken = new())
    {
    }
}
