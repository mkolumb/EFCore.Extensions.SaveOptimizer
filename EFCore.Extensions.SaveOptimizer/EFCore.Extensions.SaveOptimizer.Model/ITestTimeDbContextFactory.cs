using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Model;

public interface ITestTimeDbContextFactory<out TContext> where TContext : DbContext
{
    TContext CreateDbContext(string[] args, ILoggerFactory? factory);
}
