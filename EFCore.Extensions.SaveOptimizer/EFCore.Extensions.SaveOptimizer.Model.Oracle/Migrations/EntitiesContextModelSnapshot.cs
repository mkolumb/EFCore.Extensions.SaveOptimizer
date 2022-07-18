﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Oracle.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.AutoIncrementPrimaryKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Some")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ComposedPrimaryKeyEntity", b =>
                {
                    b.Property<int?>("PrimaryFirst")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("PrimarySecond")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Some")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("PrimaryFirst", "PrimarySecond");

                    b.ToTable("ComposedPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("NUMBER(1)");

                    b.Property<DateTimeOffset?>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTimeOffset?>("SomeNullableDateTimeProperty")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ValueConverterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("SomeHalf")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("ValueConverterEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("NUMBER(10)");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("NUMBER(1)");

                    b.Property<byte?>("SomeByte")
                        .HasColumnType("NUMBER(3)");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("TIMESTAMP(5)");

                    b.Property<DateTimeOffset?>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("BINARY_DOUBLE");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("NUMBER(10)");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("BINARY_FLOAT");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("RAW(16)");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("NUMBER(10)");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("NUMBER(19)");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("NUMBER(5)");

                    b.Property<short?>("SomeSignedByte")
                        .HasColumnType("NUMBER(3)");

                    b.Property<string>("SomeString")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("INTERVAL DAY(8) TO SECOND(7)");

                    b.Property<long?>("SomeUnsignedInt")
                        .HasColumnType("NUMBER(10)");

                    b.Property<decimal?>("SomeUnsignedLong")
                        .HasColumnType("NUMBER(20)");

                    b.Property<int?>("SomeUnsignedShort")
                        .HasColumnType("NUMBER(5)");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
