using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IDbContextDependencyResolverService
{
    IRelationalConnection GetConnection(DbContext context);

    ILogger GetLogger(DbContext context);

    IModelCacheKeyFactory GetModelCacheKeyFactory(DbContext context);
}
