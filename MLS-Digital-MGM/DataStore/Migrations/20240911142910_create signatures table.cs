using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class createsignaturestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Signatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signatures_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Signatures_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_SignatureId",
                table: "Attachments",
                column: "SignatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_CreatedById",
                table: "Signatures",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_YearOfOperationId",
                table: "Signatures",
                column: "YearOfOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Signatures_SignatureId",
                table: "Attachments",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Signatures_SignatureId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_SignatureId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "Attachments");
        }
    }
}
