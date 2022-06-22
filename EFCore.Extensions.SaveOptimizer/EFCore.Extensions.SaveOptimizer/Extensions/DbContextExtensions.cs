using EFCore.Extensions.SaveOptimizer.Models;
using EFCore.Extensions.SaveOptimizer.Resolvers;
using EFCore.Extensions.SaveOptimizer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Extensions;

public static class DbContextExtensions
{
    private static readonly QueryCompilerService _compilerService;

    private static readonly QueryTranslatorService _translatorService;

    static DbContextExtensions()
    {
        _compilerService = new QueryCompilerService(new CompilerWrapperResolver());

        _translatorService = new QueryTranslatorService();
    }

    public static int SaveChangesOptimized(this DbContext context)
    {
        var sql = context.Prepare();

        throw new NotImplementedException();
    }

    public static int SaveChangesOptimized(this DbContext context, bool acceptAllChangesOnSuccess)
    {
        var sql = context.Prepare();

        throw new NotImplementedException();
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context,
        CancellationToken cancellationToken = default)
    {
        var sql = context.Prepare();

        throw new NotImplementedException();
    }

    public static async Task<int> SaveChangesOptimizedAsync(this DbContext context, bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var sql = context.Prepare();

        throw new NotImplementedException();
    }

    private static IEnumerable<SqlResult> Prepare(this DbContext context)
    {
        var entries = context.ChangeTracker.Entries();

        QueryDataModel?[] translation = entries
            .Select(entry => _translatorService.Translate(context, entry))
            .Where(x => x != null)
            .ToArray();

        IEnumerable<SqlResult> compilation = _compilerService.Compile(translation, context.Database.ProviderName);

        return compilation;
    }
}
