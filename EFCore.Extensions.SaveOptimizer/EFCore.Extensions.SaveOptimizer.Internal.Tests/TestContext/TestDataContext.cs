﻿using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.LogEntities;
using EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable All

#pragma warning disable CS8618

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.TestContext;

public class TestDataContext : DbContext
{
    public DbSet<FirstLevelEntity> FirstLevelEntities { get; set; }
    public DbSet<ThirdLevelEntity> ThirdLevelEntities { get; set; }
    public DbSet<SecondLevelEntity> SecondLevelEntities { get; set; }
    public DbSet<AttributeEntity> AttributeEntities { get; set; }

    public DbSet<AttributeEntityLog> AttributeEntityLogs { get; set; }

    public DbSet<InsertablePrimaryKeyEntity> InsertablePrimaryKeyEntities { get; set; }
    public DbSet<NonInsertablePrimaryKeyEntity> NonInsertablePrimaryKeyEntities { get; set; }

    public TestDataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FirstLevelEntity>()
            .HasMany(i => i.AttributeEntities)
            .WithOne(x => x.FirstLevelEntity)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FirstLevelEntity>()
            .HasMany(i => i.SecondLevelEntities)
            .WithOne(x => x.FirstLevelEntity)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SecondLevelEntity>()
            .HasMany(x => x.ThirdLevelEntities)
            .WithOne(x => x.SecondLevelEntity)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AttributeEntityLog>(c => c.ToTable("attribute_log", "log"));
        modelBuilder.Entity<AttributeEntityLog>()
            .Property(c => c.EntitySnapshot)
            .HasColumnName("entity_snapshot");

        modelBuilder.Entity<AttributeEntityPropertyLog>(c => c.ToTable("attribute_property_log", "log"));
    }
}
