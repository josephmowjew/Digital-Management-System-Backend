using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedpenaltytables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    PenaltyTypeId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalties_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Penalties_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PenaltyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CPDUnitsEarned_MemberId",
                table: "CPDUnitsEarned",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_CreatedById",
                table: "Penalties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_MemberId",
                table: "Penalties",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDUnitsEarned_Members_MemberId",
                table: "CPDUnitsEarned",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDUnitsEarned_Members_MemberId",
                table: "CPDUnitsEarned");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "PenaltyTypes");

            migrationBuilder.DropIndex(
                name: "IX_CPDUnitsEarned_MemberId",
                table: "CPDUnitsEarned");
        }
    }
}
