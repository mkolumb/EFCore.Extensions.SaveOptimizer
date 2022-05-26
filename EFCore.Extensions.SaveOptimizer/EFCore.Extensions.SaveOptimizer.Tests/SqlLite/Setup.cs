using EFCore.Extensions.SaveOptimizer.Model.SqlLite;
using EFCore.Extensions.SaveOptimizer.Tests.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Tests.SqlLite;

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
