using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoIncrementEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Some = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComposedEntities",
                columns: table => new
                {
                    PrimaryFirst = table.Column<int>(type: "integer", nullable: false),
                    PrimarySecond = table.Column<int>(type: "integer", nullable: false),
                    Some = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposedEntities", x => new { x.PrimaryFirst, x.PrimarySecond });
                });

            migrationBuilder.CreateTable(
                name: "ConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SomeHalf = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConverterEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Some = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Indexer = table.Column<int>(type: "integer", nullable: false),
                    NonNullableString = table.Column<string>(type: "text", nullable: false),
                    NullableString = table.Column<string>(type: "text", nullable: true),
                    NonNullableInt = table.Column<int>(type: "integer", nullable: false),
                    NullableInt = table.Column<int>(type: "integer", nullable: true),
                    NonNullableDecimal = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    NullableDecimal = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    NonNullableDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    NullableDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NonNullableBoolean = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });

            migrationBuilder.CreateTable(
                name: "VariousTypeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SomeString = table.Column<string>(type: "text", nullable: true),
                    SomeGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    SomeBool = table.Column<bool>(type: "boolean", nullable: true),
                    SomeEnum = table.Column<int>(type: "integer", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "timestamp(5) with time zone", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<DateTimeOffset>(type: "timestamp(5) with time zone", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "interval(5)", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "smallint", nullable: true),
                    SomeUnsignedShort = table.Column<int>(type: "integer", nullable: true),
                    SomeInt = table.Column<int>(type: "integer", nullable: true),
                    SomeUnsignedInt = table.Column<long>(type: "bigint", nullable: true),
                    SomeLong = table.Column<long>(type: "bigint", nullable: true),
                    SomeUnsignedLong = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    SomeSignedByte = table.Column<short>(type: "smallint", nullable: true),
                    SomeByte = table.Column<byte>(type: "smallint", nullable: true),
                    SomeFloat = table.Column<float>(type: "real", nullable: true),
                    SomeDouble = table.Column<double>(type: "double precision", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariousTypeEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_nr_ct",
                table: "NonRelatedEntities",
                columns: new[] { "ConcurrencyToken", "NonRelatedEntityId" });

            migrationBuilder.CreateIndex(
                name: "ix_nr_idx",
                table: "NonRelatedEntities",
                column: "Indexer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoIncrementEntities");

            migrationBuilder.DropTable(
                name: "ComposedEntities");

            migrationBuilder.DropTable(
                name: "ConverterEntities");

            migrationBuilder.DropTable(
                name: "FailingEntities");

            migrationBuilder.DropTable(
                name: "NonRelatedEntities");

            migrationBuilder.DropTable(
                name: "VariousTypeEntities");
        }
    }
}
