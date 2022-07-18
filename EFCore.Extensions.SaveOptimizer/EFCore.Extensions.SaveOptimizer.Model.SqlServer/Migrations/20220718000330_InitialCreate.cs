using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoIncrementPrimaryKeyEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Some = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementPrimaryKeyEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Some = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "int", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "int", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });

            migrationBuilder.CreateTable(
                name: "ValueConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SomeHalf = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueConverterEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariousTypeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SomeString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SomeBool = table.Column<bool>(type: "bit", nullable: true),
                    SomeEnum = table.Column<int>(type: "int", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "datetime2(5)", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset(5)", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "time", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "smallint", nullable: true),
                    SomeUnsignedShort = table.Column<int>(type: "int", nullable: true),
                    SomeInt = table.Column<int>(type: "int", nullable: true),
                    SomeUnsignedInt = table.Column<long>(type: "bigint", nullable: true),
                    SomeLong = table.Column<long>(type: "bigint", nullable: true),
                    SomeUnsignedLong = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    SomeSignedByte = table.Column<short>(type: "smallint", nullable: true),
                    SomeByte = table.Column<byte>(type: "tinyint", nullable: true),
                    SomeFloat = table.Column<float>(type: "real", nullable: true),
                    SomeDouble = table.Column<double>(type: "float", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: true)
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
