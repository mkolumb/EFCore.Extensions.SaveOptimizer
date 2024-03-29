﻿using EFCore.Extensions.SaveOptimizer.Dapper.Services;
using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Factories;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Services;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Dapper;

public static class DbContextExtensions
{
    private static readonly IDbContextExecutorService DbContextExecutorService;

    static DbContextExtensions()
    {
        QueryBuilderFactory factory = new();

        QueryCompilerService compilerService = new(factory);

        QueryTranslatorService translatorService = new();

        IQueryExecutionConfiguratorService queryExecutionConfiguratorService = new QueryExecutionConfiguratorService();

        IDbContextDependencyResolverService dbContextDependencyResolverService = new DbContextDependencyResolverService();

        IQueryPreparerService queryPreparerService = new QueryPreparerService(compilerService, translatorService, dbContextDependencyResolverService);

        IQueryExecutorService queryExecutorService = new QueryExecutorService(dbContextDependencyResolverService);

        DbContextExecutorService = new DbContextExecutorService(queryPreparerService,
            queryExecutorService,
            queryExecutionConfiguratorService);
    }

    public static IExecutionResultModel SaveChangesDapperOptimized(this DbContext context) =>
        context.SaveChangesDapperOptimized(null);

    public static IExecutionResultModel SaveChangesDapperOptimized(this DbContext context,
        QueryExecutionConfiguration? configuration) =>
        DbContextExecutorService.SaveChangesOptimized(context, configuration);

    public static async Task<IExecutionResultModel> SaveChangesDapperOptimizedAsync(this DbContext context,
        CancellationToken cancellationToken = default) =>
        await context.SaveChangesDapperOptimizedAsync(null, cancellationToken).ConfigureAwait(false);

    public static async Task<IExecutionResultModel> SaveChangesDapperOptimizedAsync(this DbContext context,
        QueryExecutionConfiguration? configuration,
        CancellationToken cancellationToken = default) =>
        await DbContextExecutorService.SaveChangesOptimizedAsync(context, configuration, cancellationToken)
            .ConfigureAwait(false);
}
