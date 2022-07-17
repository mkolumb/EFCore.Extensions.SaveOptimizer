using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Cockroach.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoIncrementPrimaryKeyEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Some = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementPrimaryKeyEntities", x => x.Id);
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
                    SomeNonNullableStringProperty = table.Column<string>(type: "text", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "text", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "integer", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "integer", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "boolean", nullable: false),
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
                name: "VariousTypeEntities");
        }
    }
}
