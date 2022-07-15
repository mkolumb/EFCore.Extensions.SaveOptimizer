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
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "RAW(16)", nullable: false),
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
                name: "NonRelatedEntities");
        }
    }
}
