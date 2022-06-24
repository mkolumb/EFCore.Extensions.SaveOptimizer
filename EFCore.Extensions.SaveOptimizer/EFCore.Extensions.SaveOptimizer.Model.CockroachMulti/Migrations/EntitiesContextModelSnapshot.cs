﻿// <auto-generated />
using System;
using EFCore.Extensions.SaveOptimizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.CockroachMulti.Migrations
{
    [DbContext(typeof(EntitiesContext))]
    partial class EntitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EFCore.Extensions.SaveOptimizer.Model.NonRelatedEntity", b =>
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

                    b.ToTable("NonRelatedEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
