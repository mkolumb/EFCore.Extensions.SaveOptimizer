using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public class DbContextDependencyResolverService : IDbContextDependencyResolverService
{
    public IRelationalConnection GetConnection(DbContext context) => GetDependencies(context).RelationalConnection;

    public ILogger GetLogger(DbContext context) => GetDependencies(context).CommandLogger.Logger;

    public IModelCacheKeyFactory GetModelCacheKeyFactory(DbContext context) => context.GetService<IModelCacheKeyFactory>();

    private static IRelationalDatabaseFacadeDependencies GetDependencies(DbContext context)
    {
        IDatabaseFacadeDependenciesAccessor accessor = GetDependenciesAccessor(context);

        IDatabaseFacadeDependencies dependencies = accessor.Dependencies;

        if (dependencies is IRelationalDatabaseFacadeDependencies relationalDependencies)
        {
            return relationalDependencies;
        }

        throw new InvalidOperationException(RelationalStrings.RelationalNotInUse);
    }

    private static IDatabaseFacadeDependenciesAccessor GetDependenciesAccessor(DbContext context) => context.Database;
}
