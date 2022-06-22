using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Extensions;

public static class DbContextExtensions
{
    public static int SaveChangesOptimized(this DbContext context)
    {
        throw new NotImplementedException();
    }

    public static int SaveChangesOptimized(this DbContext context, bool acceptAllChangesOnSuccess)
    {
        throw new NotImplementedException();
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, CancellationToken cancellationToken = default)
    {
        var entries = context.ChangeTracker.Entries().ToArray();

        throw new NotImplementedException();
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
