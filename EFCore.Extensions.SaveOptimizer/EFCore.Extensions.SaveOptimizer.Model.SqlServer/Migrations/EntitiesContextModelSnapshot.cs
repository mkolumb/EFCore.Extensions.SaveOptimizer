﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.SqlServer.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.AutoIncrementPrimaryKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Some")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ComposedPrimaryKeyEntity", b =>
                {
                    b.Property<int?>("PrimaryFirst")
                        .HasColumnType("int");

                    b.Property<int?>("PrimarySecond")
                        .HasColumnType("int");

                    b.Property<string>("Some")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PrimaryFirst", "PrimarySecond");

                    b.ToTable("ComposedPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("datetimeoffset");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("SomeNullableDateTimeProperty")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("int");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ValueConverterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SomeHalf")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ValueConverterEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("bit");

                    b.Property<byte?>("SomeByte")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("datetime2(5)");

                    b.Property<DateTimeOffset?>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("datetimeoffset(5)");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("float");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("int");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("real");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("int");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("bigint");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("smallint");

                    b.Property<short?>("SomeSignedByte")
                        .HasColumnType("smallint");

                    b.Property<string>("SomeString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("time");

                    b.Property<long?>("SomeUnsignedInt")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("SomeUnsignedLong")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int?>("SomeUnsignedShort")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
