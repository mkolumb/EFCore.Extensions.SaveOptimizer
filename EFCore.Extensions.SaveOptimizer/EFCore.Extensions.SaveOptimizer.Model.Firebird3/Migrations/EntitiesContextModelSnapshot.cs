﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Firebird3.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 30);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.AutoIncrementEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Some")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("Id");

                    b.ToTable("AutoIncrementEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ComposedEntity", b =>
                {
                    b.Property<int?>("PrimaryFirst")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PrimarySecond")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Some")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("PrimaryFirst", "PrimarySecond");

                    b.ToTable("ComposedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.ConverterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(16) CHARACTER SET OCTETS");

                    b.Property<string>("SomeHalf")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("Id");

                    b.ToTable("ConverterEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.FailingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Some")
                        .IsRequired()
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("Id");

                    b.ToTable("FailingEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(16) CHARACTER SET OCTETS");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("VARCHAR(48)");

                    b.Property<int>("Indexer")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("NonNullableBoolean")
                        .IsRequired()
                        .HasColumnType("BOOLEAN");

                    b.Property<string>("NonNullableDateTime")
                        .IsRequired()
                        .HasColumnType("VARCHAR(48)");

                    b.Property<decimal?>("NonNullableDecimal")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("NonNullableInt")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NonNullableString")
                        .IsRequired()
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.Property<string>("NullableDateTime")
                        .HasColumnType("VARCHAR(48)");

                    b.Property<decimal?>("NullableDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("NullableInt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NullableString")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex(new[] { "ConcurrencyToken", "NonRelatedEntityId" }, "ix_nr_ct");

                    b.HasIndex(new[] { "Indexer" }, "ix_nr_idx");

                    b.ToTable("NonRelatedEntities");
                });

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.Entities.VariousTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("SomeBool")
                        .HasColumnType("BOOLEAN");

                    b.Property<short?>("SomeByte")
                        .HasColumnType("SMALLINT");

                    b.Property<DateTime?>("SomeDateTime")
                        .HasPrecision(5)
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("SomeDateTimeOffset")
                        .HasPrecision(5)
                        .HasColumnType("VARCHAR(48)");

                    b.Property<decimal?>("SomeDecimal")
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<double?>("SomeDouble")
                        .HasColumnType("DOUBLE PRECISION");

                    b.Property<int?>("SomeEnum")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("SomeFloat")
                        .HasColumnType("FLOAT");

                    b.Property<Guid?>("SomeGuid")
                        .HasColumnType("CHAR(16) CHARACTER SET OCTETS");

                    b.Property<int?>("SomeInt")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("SomeLong")
                        .HasColumnType("BIGINT");

                    b.Property<short?>("SomeShort")
                        .HasColumnType("SMALLINT");

                    b.Property<short?>("SomeSignedByte")
                        .HasColumnType("SMALLINT");

                    b.Property<string>("SomeString")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.Property<TimeSpan?>("SomeTimeSpan")
                        .HasPrecision(5)
                        .HasColumnType("TIME");

                    b.Property<long?>("SomeUnsignedInt")
                        .HasColumnType("BIGINT");

                    b.Property<decimal?>("SomeUnsignedLong")
                        .HasColumnType("DECIMAL(18,2)");

                    b.Property<int?>("SomeUnsignedShort")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("VariousTypeEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
