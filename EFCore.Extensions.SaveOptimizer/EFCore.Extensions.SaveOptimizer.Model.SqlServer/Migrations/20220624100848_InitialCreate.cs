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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonRelatedEntities");
        }
    }
}
