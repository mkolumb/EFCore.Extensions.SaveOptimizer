using System.Data;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public sealed class DbContextWrapper : IDisposable
{
    private readonly IDesignTimeDbContextFactory<EntitiesContext> _factory;

    public EntitiesContext Context { get; private set; }

    public DbContextWrapper(IDesignTimeDbContextFactory<EntitiesContext> factory)
    {
        _factory = factory;

        Context = _factory.CreateDbContext(Array.Empty<string>());
    }

    public void Dispose() => Context.Dispose();

    public void RecreateContext()
    {
        var connectionString = Context.Database.GetConnectionString();

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        Context.Dispose();

        Context = _factory.CreateDbContext(new[] { connectionString });
    }

    public async Task Save(SaveVariant variant)
    {
        async Task InternalSave()
        {
            if ((variant & SaveVariant.Optimized) != 0)
            {
                await Context.SaveChangesOptimizedAsync();
            }
            else if ((variant & SaveVariant.EfCore) != 0)
            {
                await Context.SaveChangesAsync();
            }
        }

        if ((variant & SaveVariant.WithTransaction) != 0)
        {
            await using IDbContextTransaction transaction =
                await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            await InternalSave();

            await transaction.CommitAsync();
        }
        else
        {
            await InternalSave();
        }

        if ((variant & SaveVariant.Recreate) != 0)
        {
            RecreateContext();
        }
    }
}
