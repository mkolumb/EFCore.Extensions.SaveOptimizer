using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Model;

public class EntitiesContext : DbContext
{
    public DbSet<NonRelatedEntity> NonRelatedEntities { get; set; }

    public EntitiesContext(DbContextOptions<EntitiesContext> options)
        : base(options)
    {
    }
}
