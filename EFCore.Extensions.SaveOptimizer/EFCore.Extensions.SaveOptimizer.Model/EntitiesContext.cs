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

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<NonRelatedEntity>(
            eb =>
            {
                eb.Property(b => b.SomeNullableDecimalProperty).HasPrecision(12, 6);
                eb.Property(b => b.SomeNonNullableDecimalProperty).HasPrecision(12, 6);
            });
}
