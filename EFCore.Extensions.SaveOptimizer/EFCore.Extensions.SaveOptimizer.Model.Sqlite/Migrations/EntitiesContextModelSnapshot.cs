﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Sqlite.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.AutoIncrementPrimaryKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Some")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementPrimaryKeyEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("TEXT");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("SomeNullableDateTimeProperty")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("TEXT");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("TEXT");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("SomeByte")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("TEXT");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("REAL");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("REAL");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("INTEGER");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("INTEGER");

                    b.Property<sbyte?>("SomeSignedByte")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SomeString")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("TEXT");

                    b.Property<uint?>("SomeUnsignedInt")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("SomeUnsignedLong")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("SomeUnsignedShort")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
