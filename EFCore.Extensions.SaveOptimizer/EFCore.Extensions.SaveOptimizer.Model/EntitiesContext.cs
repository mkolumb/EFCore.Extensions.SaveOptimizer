﻿using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Model;

public class EntitiesContext : DbContext
{
    public DbSet<NonRelatedEntity> NonRelatedEntities { get; set; }

    public EntitiesContext(DbContextOptions<EntitiesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.ProviderName != null && Database.ProviderName.Contains("Firebird"))
        {
            const string columnType = "DECIMAL(12,6)";

            modelBuilder.Entity<NonRelatedEntity>(
                eb =>
                {
                    eb.Property(b => b.SomeNullableDecimalProperty)
                        .HasPrecision(12, 6)
                        .HasColumnType(columnType);

                    eb.Property(b => b.SomeNonNullableDecimalProperty)
                        .HasPrecision(12, 6)
                        .HasColumnType(columnType);
                });
        }
        else
        {
            modelBuilder.Entity<NonRelatedEntity>(
                eb =>
                {
                    eb.Property(b => b.SomeNullableDecimalProperty)
                        .HasPrecision(12, 6);

                    eb.Property(b => b.SomeNonNullableDecimalProperty)
                        .HasPrecision(12, 6);
                });
        }

        modelBuilder.Entity<NonRelatedEntity>()
            .HasIndex(x => new { x.ConcurrencyToken, x.NonRelatedEntityId })
            .IsUnique(false);
    }
}
