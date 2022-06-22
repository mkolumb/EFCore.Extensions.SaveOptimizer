﻿using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark;

public class DbContextWrapper : DbContextWrapperBase
{
    public DbContextWrapper(IDbContextFactory<EntitiesContext> factory) : base(factory)
    {
    }

    public override async Task Truncate()
    {
        var query = "DELETE FROM NonRelatedEntities";

        await Context.Database.ExecuteSqlRawAsync(query);
    }
}
