using System.Data;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class DbContextWrapperBase : IDbContextWrapper
{
    private readonly IDbContextFactory<EntitiesContext> _factory;

    protected DbContextWrapperBase(IDbContextFactory<EntitiesContext> factory)
    {
        _factory = factory;

        Context = _factory.CreateDbContext();
    }

    public EntitiesContext Context { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void RecreateContext()
    {
        var connectionString = Context.Database.GetConnectionString();

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        Context.Dispose();

        Context = _factory.CreateDbContext();
    }

    public abstract Task Truncate();

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

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    }
}
