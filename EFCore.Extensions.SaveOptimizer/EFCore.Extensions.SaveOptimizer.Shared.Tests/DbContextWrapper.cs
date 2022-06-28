using System.Data;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Constants;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.TestLogger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests;

public sealed class DbContextWrapper : IDisposable
{
    private readonly ITestTimeDbContextFactory<EntitiesContext> _factory;
    private readonly ILoggerFactory _loggerFactory;

    public EntitiesContext Context { get; private set; }

    public DbContextWrapper(ITestTimeDbContextFactory<EntitiesContext> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _loggerFactory = new LoggerFactory(new[] { new TestLoggerProvider(testOutputHelper, LogLevel.Warning) });

        Context = _factory.CreateDbContext(Array.Empty<string>(), _loggerFactory);
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

        Context = _factory.CreateDbContext(new[] { connectionString }, _loggerFactory);
    }

    public async Task Save(SaveVariant variant, int batchSize = InternalConstants.DefaultBatchSize)
    {
        async Task InternalSave()
        {
            if ((variant & SaveVariant.Optimized) != 0)
            {
                await Context.SaveChangesOptimizedAsync(new QueryExecutionConfiguration { BatchSize = batchSize });
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
