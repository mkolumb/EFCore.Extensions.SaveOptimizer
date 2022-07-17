using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Model;

public class EntitiesContext : DbContext
{
    public static List<Action<ModelBuilder>> AdditionalBuilders = new();

    public DbSet<NonRelatedEntity> NonRelatedEntities { get; set; }

    public DbSet<AutoIncrementPrimaryKeyEntity> AutoIncrementPrimaryKeyEntities { get; set; }

    public DbSet<VariousTypeEntity> VariousTypeEntities { get; set; }

    public DbSet<FailingEntity> FailingEntities { get; set; }

    public EntitiesContext(DbContextOptions<EntitiesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.ConcurrencyToken, x.NonRelatedEntityId })
            .IsUnique(false);

        foreach (Action<ModelBuilder> builder in AdditionalBuilders)
        {
            builder(modelBuilder);
        }
    }
}
