using System.Data.Common;
using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark;

public class InsertBenchmark : BaseInsertBenchmark
{
    [Params(1L, 10L, 100L, 1000L, 10000L)]
    public override long Rows { get; set; }

    public InsertBenchmark() : base(ContextResolver())
    {
    }

    private static IWrapperResolver ContextResolver()
    {
        DbConnection connection = GetConnection();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder.UseNpgsql(connection,
            cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Postgres")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    private static DbConnection GetConnection()
    {
        const string connectionString = "Host=localhost;Port=26257;SSL Mode=Disable;Username=root;Database=test_db";

        NpgsqlConnection conn = new(connectionString);

        conn.UserCertificateValidationCallback += (_, _, _, _) => true;

        return conn;
    }
}
