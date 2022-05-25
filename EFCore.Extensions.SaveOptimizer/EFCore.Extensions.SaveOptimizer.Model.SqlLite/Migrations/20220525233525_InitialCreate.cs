using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.SqlLite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "TEXT", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "TEXT", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "TEXT", nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "TEXT", nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConcurrencyToken = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonRelatedEntities");
        }
    }
}
