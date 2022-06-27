using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMySql.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SomeNonNullableStringProperty = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SomeNullableStringProperty = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SomeNonNullableIntProperty = table.Column<int>(type: "int", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "int", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "decimal(12,6)", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_ConcurrencyToken_NonRelatedEntityId",
                table: "NonRelatedEntities",
                columns: new[] { "ConcurrencyToken", "NonRelatedEntityId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonRelatedEntities");
        }
    }
}
