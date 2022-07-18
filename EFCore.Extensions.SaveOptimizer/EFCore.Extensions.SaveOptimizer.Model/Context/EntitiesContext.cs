using EFCore.Extensions.SaveOptimizer.Model.Converters;
using EFCore.Extensions.SaveOptimizer.Model.Entities;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Model.Context;

public class EntitiesContext : DbContext
{
    public static List<Action<ModelBuilder>> AdditionalBuilders { get; } = new();

    public DbSet<NonRelatedEntity> NonRelatedEntities { get; set; }

    public DbSet<AutoIncrementPrimaryKeyEntity> AutoIncrementPrimaryKeyEntities { get; set; }

    public DbSet<VariousTypeEntity> VariousTypeEntities { get; set; }

    public DbSet<FailingEntity> FailingEntities { get; set; }

    public DbSet<ValueConverterEntity> ValueConverterEntities { get; set; }

    public DbSet<ComposedPrimaryKeyEntity> ComposedPrimaryKeyEntities { get; set; }

    public EntitiesContext(DbContextOptions<EntitiesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.ConcurrencyToken, x.NonRelatedEntityId })
            .IsUnique(false);

        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.Indexer })
            .IsUnique(false);

        modelBuilder.Entity<ValueConverterEntity>()
            .Property(x => x.SomeHalf)
            .HasConversion<HalfValueConverter>();

        modelBuilder.Entity<ComposedPrimaryKeyEntity>()
            .HasKey(x => new { x.PrimaryFirst, x.PrimarySecond });

        foreach (Action<ModelBuilder> builder in AdditionalBuilders)
        {
            builder(modelBuilder);
        }
    }
}
