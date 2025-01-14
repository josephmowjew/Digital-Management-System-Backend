using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddInstitutionTypetoFirmsTableandaddedInstitutionTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitutionTypeId",
                table: "Firms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InstitutionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Firms_InstitutionTypeId",
                table: "Firms",
                column: "InstitutionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_InstitutionTypes_InstitutionTypeId",
                table: "Firms",
                column: "InstitutionTypeId",
                principalTable: "InstitutionTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_InstitutionTypes_InstitutionTypeId",
                table: "Firms");

            migrationBuilder.DropTable(
                name: "InstitutionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Firms_InstitutionTypeId",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "InstitutionTypeId",
                table: "Firms");
        }
    }
}
