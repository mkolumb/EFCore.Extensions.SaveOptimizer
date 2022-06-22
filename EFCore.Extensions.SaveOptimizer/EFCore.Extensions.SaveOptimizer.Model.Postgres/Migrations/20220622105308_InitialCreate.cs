using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "text", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "text", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "integer", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "integer", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "numeric", nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "numeric", nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
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
