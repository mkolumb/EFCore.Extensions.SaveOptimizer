using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Sqlite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoIncrementPrimaryKeyEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Some = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementPrimaryKeyEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComposedPrimaryKeyEntities",
                columns: table => new
                {
                    PrimaryFirst = table.Column<string>(type: "TEXT", nullable: false),
                    PrimarySecond = table.Column<string>(type: "TEXT", nullable: false),
                    Some = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposedPrimaryKeyEntities", x => new { x.PrimaryFirst, x.PrimarySecond });
                });

            migrationBuilder.CreateTable(
                name: "FailingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Some = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "TEXT", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "TEXT", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });

            migrationBuilder.CreateTable(
                name: "ValueConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SomeHalf = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueConverterEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariousTypeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    SomeString = table.Column<string>(type: "TEXT", nullable: true),
                    SomeGuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    SomeBool = table.Column<bool>(type: "INTEGER", nullable: true),
                    SomeEnum = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "TEXT", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<DateTimeOffset>(type: "TEXT", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "TEXT", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "INTEGER", nullable: true),
                    SomeUnsignedShort = table.Column<ushort>(type: "INTEGER", nullable: true),
                    SomeInt = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeUnsignedInt = table.Column<uint>(type: "INTEGER", nullable: true),
                    SomeLong = table.Column<long>(type: "INTEGER", nullable: true),
                    SomeUnsignedLong = table.Column<ulong>(type: "INTEGER", nullable: true),
                    SomeSignedByte = table.Column<sbyte>(type: "INTEGER", nullable: true),
                    SomeByte = table.Column<byte>(type: "INTEGER", nullable: true),
                    SomeFloat = table.Column<float>(type: "REAL", nullable: true),
                    SomeDouble = table.Column<double>(type: "REAL", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "TEXT", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariousTypeEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_ConcurrencyToken_NonRelatedEntityId",
                table: "NonRelatedEntities",
                columns: new[] { "ConcurrencyToken", "NonRelatedEntityId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoIncrementPrimaryKeyEntities");

            migrationBuilder.DropTable(
                name: "ComposedPrimaryKeyEntities");

            migrationBuilder.DropTable(
                name: "FailingEntities");

            migrationBuilder.DropTable(
                name: "NonRelatedEntities");

            migrationBuilder.DropTable(
                name: "ValueConverterEntities");

            migrationBuilder.DropTable(
                name: "VariousTypeEntities");
        }
    }
}
