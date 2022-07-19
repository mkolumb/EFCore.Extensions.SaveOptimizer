using EFCore.Extensions.SaveOptimizer.Model.Converters;
using EFCore.Extensions.SaveOptimizer.Model.Entities;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Model.Context;

public class EntitiesContext : DbContext
{
    public static List<Action<ModelBuilder>> AdditionalBuilders { get; } = new();

    public DbSet<NonRelatedEntity> NonRelatedEntities { get; set; }

    public DbSet<AutoIncrementEntity> AutoIncrementEntities { get; set; }

    public DbSet<VariousTypeEntity> VariousTypeEntities { get; set; }

    public DbSet<FailingEntity> FailingEntities { get; set; }

    public DbSet<ConverterEntity> ConverterEntities { get; set; }

    public DbSet<ComposedEntity> ComposedEntities { get; set; }

    public EntitiesContext(DbContextOptions<EntitiesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.ConcurrencyToken, x.NonRelatedEntityId }, "ix_nr_ct")
            .IsUnique(false);

        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.Indexer }, "ix_nr_idx")
            .IsUnique(false);

        modelBuilder.Entity<ConverterEntity>()
            .Property(x => x.SomeHalf)
            .HasConversion<HalfValueConverter>();

        modelBuilder.Entity<ComposedEntity>()
            .HasKey(x => new { x.PrimaryFirst, x.PrimarySecond });

        modelBuilder.Model.SetMaxIdentifierLength(30);

        foreach (Action<ModelBuilder> builder in AdditionalBuilders)
        {
            builder(modelBuilder);
        }
    }
}
