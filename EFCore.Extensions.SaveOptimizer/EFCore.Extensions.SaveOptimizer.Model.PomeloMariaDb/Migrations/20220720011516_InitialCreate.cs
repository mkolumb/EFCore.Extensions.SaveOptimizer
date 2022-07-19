using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutoIncrementEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Some = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComposedEntities",
                columns: table => new
                {
                    PrimaryFirst = table.Column<int>(type: "int", nullable: false),
                    PrimarySecond = table.Column<int>(type: "int", nullable: false),
                    Some = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposedEntities", x => new { x.PrimaryFirst, x.PrimarySecond });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SomeHalf = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConverterEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FailingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Some = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Indexer = table.Column<int>(type: "int", nullable: false),
                    NonNullableString = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NullableString = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NonNullableInt = table.Column<int>(type: "int", nullable: false),
                    NullableInt = table.Column<int>(type: "int", nullable: true),
                    NonNullableDecimal = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: false),
                    NullableDecimal = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: true),
                    NonNullableDateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    NullableDateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    NonNullableBoolean = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VariousTypeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SomeString = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SomeGuid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SomeBool = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    SomeEnum = table.Column<int>(type: "int", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "datetime(5)", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<DateTimeOffset>(type: "datetime(5)", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "time(5)", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "smallint", nullable: true),
                    SomeUnsignedShort = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    SomeInt = table.Column<int>(type: "int", nullable: true),
                    SomeUnsignedInt = table.Column<uint>(type: "int unsigned", nullable: true),
                    SomeLong = table.Column<long>(type: "bigint", nullable: true),
                    SomeUnsignedLong = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    SomeSignedByte = table.Column<sbyte>(type: "tinyint", nullable: true),
                    SomeByte = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    SomeFloat = table.Column<float>(type: "float", nullable: true),
                    SomeDouble = table.Column<double>(type: "double", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariousTypeEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
