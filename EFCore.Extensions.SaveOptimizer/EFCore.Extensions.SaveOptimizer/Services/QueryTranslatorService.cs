using EFCore.Extensions.SaveOptimizer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Services;

public class QueryTranslatorService : IQueryTranslatorService
{
    public QueryDataModel Translate<TContext, TEntity>(EntityEntry entry)
        where TContext : DbContext
        where TEntity : class => throw new NotImplementedException();
}
