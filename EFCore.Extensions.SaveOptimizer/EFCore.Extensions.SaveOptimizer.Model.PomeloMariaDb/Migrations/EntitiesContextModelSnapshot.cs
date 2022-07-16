﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.AutoIncrementPrimaryKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Some")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.FailingEntity", b =>
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

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("SomeNullableDateTimeProperty")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("decimal(12,6)");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("int");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("longtext");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.VariousTypeEntity", b =>
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
