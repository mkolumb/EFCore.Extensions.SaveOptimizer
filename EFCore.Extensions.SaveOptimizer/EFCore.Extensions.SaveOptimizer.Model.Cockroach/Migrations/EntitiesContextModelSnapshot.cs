﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Cockroach.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.AutoIncrementPrimaryKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Some")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ComposedPrimaryKeyEntity", b =>
                {
                    b.Property<int?>("PrimaryFirst")
                        .HasColumnType("integer");

                    b.Property<int?>("PrimarySecond")
                        .HasColumnType("integer");

                    b.Property<string>("Some")
                        .HasColumnType("text");

                    b.HasKey("PrimaryFirst", "PrimarySecond");

                    b.ToTable("ComposedPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("numeric(12,6)");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("SomeNullableDateTimeProperty")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("numeric(12,6)");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("integer");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("text");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ValueConverterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("SomeHalf")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ValueConverterEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("boolean");

                    b.Property<byte?>("SomeByte")
                        .HasColumnType("smallint");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("timestamp(5) with time zone");

                    b.Property<DateTimeOffset?>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("timestamp(5) with time zone");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("numeric(12,6)");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("double precision");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("integer");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("real");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("uuid");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("integer");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("bigint");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("smallint");

                    b.Property<short?>("SomeSignedByte")
                        .HasColumnType("smallint");

                    b.Property<string>("SomeString")
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("interval(5)");

                    b.Property<long?>("SomeUnsignedInt")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("SomeUnsignedLong")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int?>("SomeUnsignedShort")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
