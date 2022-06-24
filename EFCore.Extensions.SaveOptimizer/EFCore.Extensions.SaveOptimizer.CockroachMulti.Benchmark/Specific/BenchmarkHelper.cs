using System.Data.Common;
using EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark.Standard;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark.Specific;

public static class BenchmarkHelper
{
    public static IWrapperResolver ContextResolver()
    {
        DbConnection connection = GetConnection();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder.UseNpgsql(connection,
            cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.CockroachMulti")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    public static DbConnection GetConnection()
    {
        const string connectionString = "Host=localhost;Port=26259;SSL Mode=Disable;Username=root;Database=test_db";

        NpgsqlConnection conn = new(connectionString);

        conn.UserCertificateValidationCallback += (_, _, _, _) => true;

        return conn;
    }
}
