using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using EFCore.Extensions.SaveOptimizer.Services;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer;

public static class DbContextExtensions
{
    private static readonly IDbContextExecutorService DbContextExecutorService;

    static DbContextExtensions()
    {
        QueryBuilderFactory factory = new();

        QueryCompilerService compilerService = new(factory);

        QueryTranslatorService translatorService = new();

        IQueryExecutionConfiguratorService queryExecutionConfiguratorService = new QueryExecutionConfiguratorService();

        IQueryPreparerService queryPreparerService = new QueryPreparerService(compilerService, translatorService);

        IDbContextDependencyResolverService dbContextDependencyResolverService =
            new DbContextDependencyResolverService();

        IQueryExecutorService queryExecutorService = new QueryExecutorService(dbContextDependencyResolverService);

        DbContextExecutorService = new DbContextExecutorService(queryPreparerService,
            queryExecutorService,
            queryExecutionConfiguratorService);
    }

    public static IExecutionResultModel SaveChangesOptimized(this DbContext context) =>
        context.SaveChangesOptimized(null);

    public static IExecutionResultModel SaveChangesOptimized(this DbContext context,
        QueryExecutionConfiguration? configuration) =>
        DbContextExecutorService.SaveChangesOptimized(context, configuration);

    public static async Task<IExecutionResultModel> SaveChangesOptimizedAsync(this DbContext context,
        CancellationToken cancellationToken = default) =>
        await context.SaveChangesOptimizedAsync(null, cancellationToken).ConfigureAwait(false);

    public static async Task<IExecutionResultModel> SaveChangesOptimizedAsync(this DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken = default) =>
        await DbContextExecutorService.SaveChangesOptimizedAsync(context, configuration, cancellationToken)
            .ConfigureAwait(false);
}
