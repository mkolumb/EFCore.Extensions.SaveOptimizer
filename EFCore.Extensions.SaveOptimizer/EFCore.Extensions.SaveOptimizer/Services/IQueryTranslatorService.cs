using EFCore.Extensions.SaveOptimizer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Services;

public interface IQueryTranslatorService
{
    QueryDataModel? Translate<TContext, TEntity>(EntityEntry entry)
        where TContext : DbContext
        where TEntity : class;
}
