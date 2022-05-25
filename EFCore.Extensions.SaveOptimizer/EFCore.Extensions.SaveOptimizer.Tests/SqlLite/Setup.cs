using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Model.SqlLite;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Tests.SqlLite;

public abstract class Setup
{
    protected EntitiesContext ContextResolver()
    {
        SqlLiteDesignTimeFactory factory = new();

        EntitiesContext context = factory.CreateDbContext(Array.Empty<string>());

        context.Database.Migrate();

        return context;
    }
}
