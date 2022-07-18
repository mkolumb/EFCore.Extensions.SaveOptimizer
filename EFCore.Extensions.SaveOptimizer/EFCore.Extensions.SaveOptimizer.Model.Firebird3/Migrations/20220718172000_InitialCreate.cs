using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Extensions.SaveOptimizer.Model.Firebird3.Migrations
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
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Some = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoIncrementPrimaryKeyEnti~", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComposedPrimaryKeyEntities",
                columns: table => new
                {
                    PrimaryFirst = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimarySecond = table.Column<int>(type: "INTEGER", nullable: false),
                    Some = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
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
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Some = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailingEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "CHAR(16) CHARACTER SET OCTETS", nullable: false),
                    Indexer = table.Column<int>(type: "INTEGER", nullable: false),
                    SomeNonNullableStringProperty = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: false),
                    SomeNullableStringProperty = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SomeNonNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: false),
                    SomeNullableIntProperty = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeNonNullableDecimalProperty = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: false),
                    SomeNullableDecimalProperty = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: true),
                    SomeNonNullableDateTimeProperty = table.Column<string>(type: "VARCHAR(48)", nullable: false),
                    SomeNullableDateTimeProperty = table.Column<string>(type: "VARCHAR(48)", nullable: true),
                    SomeNonNullableBooleanProperty = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "VARCHAR(48)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonRelatedEntities", x => x.NonRelatedEntityId);
                });

            migrationBuilder.CreateTable(
                name: "ValueConverterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "CHAR(16) CHARACTER SET OCTETS", nullable: false),
                    SomeHalf = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
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
                    SomeString = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SomeGuid = table.Column<Guid>(type: "CHAR(16) CHARACTER SET OCTETS", nullable: true),
                    SomeBool = table.Column<bool>(type: "BOOLEAN", nullable: true),
                    SomeEnum = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeDateTime = table.Column<DateTime>(type: "TIMESTAMP", precision: 5, nullable: true),
                    SomeDateTimeOffset = table.Column<string>(type: "VARCHAR(48)", precision: 5, nullable: true),
                    SomeTimeSpan = table.Column<TimeSpan>(type: "TIME", precision: 5, nullable: true),
                    SomeShort = table.Column<short>(type: "SMALLINT", nullable: true),
                    SomeUnsignedShort = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeInt = table.Column<int>(type: "INTEGER", nullable: true),
                    SomeUnsignedInt = table.Column<long>(type: "BIGINT", nullable: true),
                    SomeLong = table.Column<long>(type: "BIGINT", nullable: true),
                    SomeUnsignedLong = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    SomeSignedByte = table.Column<short>(type: "SMALLINT", nullable: true),
                    SomeByte = table.Column<short>(type: "SMALLINT", nullable: true),
                    SomeFloat = table.Column<float>(type: "FLOAT", nullable: true),
                    SomeDouble = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    SomeDecimal = table.Column<decimal>(type: "DECIMAL(12,6)", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariousTypeEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_Concurre~",
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
