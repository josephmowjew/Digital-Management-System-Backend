using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class Addedfirmstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Firms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PhysicalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PrimaryContactPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PrimaryPhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    SecondaryContactPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    SecondaryPhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firms", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirmId",
                table: "Users",
                column: "FirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Firms_FirmId",
                table: "Users",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Firms_FirmId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_Users_FirmId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Users");
        }
    }
}
