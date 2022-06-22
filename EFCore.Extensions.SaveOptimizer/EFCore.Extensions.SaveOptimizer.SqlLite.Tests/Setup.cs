using EFCore.Extensions.SaveOptimizer.Model.SqlLite;
using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Tests;

public abstract class Setup
{
    protected DbContextWrapper ContextWrapperResolver()
    {
        SqlLiteDesignTimeFactory factory = new();

        DbContextWrapper wrapper = new(factory);

        wrapper.Context.Database.Migrate();

        return wrapper;
    }
}
