﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model;
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
                .HasAnnotation("Relational:MaxIdentifierLength", 31);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.NonRelatedEntity", b =>
                {
                    b.Property<Guid>("NonRelatedEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(16) CHARACTER SET OCTETS");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("VARCHAR(48)");

                    b.Property<bool?>("SomeNonNullableBooleanProperty")
                        .IsRequired()
                        .HasColumnType("BOOLEAN");

                    b.Property<string>("SomeNonNullableDateTimeProperty")
                        .IsRequired()
                        .HasColumnType("VARCHAR(48)");

                    b.Property<decimal?>("SomeNonNullableDecimalProperty")
                        .IsRequired()
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("SomeNonNullableIntProperty")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SomeNonNullableStringProperty")
                        .IsRequired()
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.Property<string>("SomeNullableDateTimeProperty")
                        .HasColumnType("VARCHAR(48)");

                    b.Property<decimal?>("SomeNullableDecimalProperty")
                        .HasPrecision(12, 6)
                        .HasColumnType("DECIMAL(12,6)");

                    b.Property<int?>("SomeNullableIntProperty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SomeNullableStringProperty")
                        .HasColumnType("BLOB SUB_TYPE TEXT");

                    b.HasKey("NonRelatedEntityId");

                    b.HasIndex("ConcurrencyToken", "NonRelatedEntityId");

                    b.ToTable("NonRelatedEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
