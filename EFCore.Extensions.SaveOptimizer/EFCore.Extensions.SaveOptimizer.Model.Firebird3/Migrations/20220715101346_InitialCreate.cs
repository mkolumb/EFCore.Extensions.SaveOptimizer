﻿using System;
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
                name: "NonRelatedEntities",
                columns: table => new
                {
                    NonRelatedEntityId = table.Column<Guid>(type: "CHAR(16) CHARACTER SET OCTETS", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_NonRelatedEntities_Concurre~",
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