using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;

public class TestDataContext : DbContext
{
    public DbSet<PerformanceEntity> PerformanceEntities { get; set; }

    public TestDataContext(DbContextOptions options) : base(options)
    {
    }
}
