﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    [Migration("20220720011516_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 30);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.AutoIncrementEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Some")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ComposedEntity", b =>
                {
                    b.Property<int?>("PrimaryFirst")
                        .HasColumnType("int");

                    b.Property<int?>("PrimarySecond")
                        .HasColumnType("int");

                    b.Property<string>("Some")
                        .HasColumnType("longtext");

                    b.HasKey("PrimaryFirst", "PrimarySecond");

                    b.ToTable("ComposedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ConverterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("SomeHalf")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ConverterEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Indexer")
                        .HasColumnType("int");

                    b.Property<bool?>("NonNullableBoolean")
                        .IsRequired()
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("NonNullableDateTime")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("NonNullableDecimal")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("NonNullableInt")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("NonNullableString")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("NullableDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("NullableDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("NullableInt")
                        .HasColumnType("int");

                    b.Property<string>("NullableString")
                        .HasColumnType("longtext");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex(new[] { "ConcurrencyToken", "NonRelatedEntityId" }, "ix_nr_ct");

                    b.HasIndex(new[] { "Indexer" }, "ix_nr_idx");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("tinyint(1)");

                    b.Property<byte?>("SomeByte")
                        .HasColumnType("tinyint unsigned");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("datetime(5)");

                    b.Property<DateTimeOffset?>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("datetime(5)");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("double");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("int");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("float");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("char(36)");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("int");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("bigint");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("smallint");

                    b.Property<sbyte?>("SomeSignedByte")
                        .HasColumnType("tinyint");

                    b.Property<string>("SomeString")
                        .HasColumnType("longtext");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("time(5)");

                    b.Property<uint?>("SomeUnsignedInt")
                        .HasColumnType("int unsigned");

                    b.Property<ulong?>("SomeUnsignedLong")
                        .HasColumnType("bigint unsigned");

                    b.Property<ushort?>("SomeUnsignedShort")
                        .HasColumnType("smallint unsigned");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}