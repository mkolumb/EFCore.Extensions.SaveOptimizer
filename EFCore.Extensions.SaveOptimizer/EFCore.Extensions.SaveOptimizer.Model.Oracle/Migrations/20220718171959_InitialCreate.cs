using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Oracle.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoIncrementPrimaryKeyEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Some = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementPrimaryKeyEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComposedPrimaryKeyEntities",
                columns: table => new
                {
                    PrimaryFirst = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PrimarySecond = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Some = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposedPrimaryKeyEntities", x => new { x.PrimaryFirst, x.PrimarySecond });
                });

            migrationBuilder.CreateTable(
                name: "FailingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Some = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Indexer = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });

            migrationBuilder.CreateTable(
                name: "ValueConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SomeHalf = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueConverterEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariousTypeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SomeString = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SomeGuid = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    SomeBool = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SomeEnum = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "TIMESTAMP(5)", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "INTERVAL DAY(8) TO SECOND(7)", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "NUMBER(5)", nullable: true),
                    SomeUnsignedShort = table.Column<int>(type: "NUMBER(5)", nullable: true),
                    SomeInt = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SomeUnsignedInt = table.Column<long>(type: "NUMBER(10)", nullable: true),
                    SomeLong = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SomeUnsignedLong = table.Column<decimal>(type: "NUMBER(20)", nullable: true),
                    SomeSignedByte = table.Column<short>(type: "NUMBER(3)", nullable: true),
                    SomeByte = table.Column<byte>(type: "NUMBER(3)", nullable: true),
                    SomeFloat = table.Column<float>(type: "BINARY_FLOAT", nullable: true),
                    SomeDouble = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariousTypeEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_ConcurrencyToken_NonRelatedEntityId",
                table: "NonRelatedEntities",
                columns: new[] { "ConcurrencyToken", "NonRelatedEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_Indexer",
                table: "NonRelatedEntities",
                column: "Indexer");
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
