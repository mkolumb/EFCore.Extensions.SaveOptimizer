using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark;

public class InsertBenchmark : BaseInsertBenchmark
{
    [Params(1L, 10L, 100L)]
    public override long Rows { get; set; }

    public InsertBenchmark() : base(ContextResolver())
    {
    }

    private static IWrapperResolver ContextResolver()
    {
        var connectionString = GetConnectionString();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder.UseSqlite(connectionString,
            cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlLite")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    private static string GetConnectionString()
    {
        DirectoryInfo directory = new(Path.Combine(Path.GetTempPath(), "db"));

        if (!directory.Exists)
        {
            directory.Create();
        }

        var dbName = $"test_{DateTime.Now:yyyy_MM_dd_hh_mm_ss}_{Guid.NewGuid()}.db";

        var path = Path.Join(directory.FullName, dbName);

        return $"Data Source={path}";
    }
}
