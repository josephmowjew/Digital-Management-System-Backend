using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedpenaltypaymenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "Penalties",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PenaltyStatus",
                table: "Penalties",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResolutionComment",
                table: "Penalties",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfOperationId",
                table: "Penalties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyPaymentId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PenaltyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PenaltyId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PenaltyPayments_Penalties_PenaltyId",
                        column: x => x.PenaltyId,
                        principalTable: "Penalties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_YearOfOperationId",
                table: "Penalties",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PenaltyId",
                table: "Attachments",
                column: "PenaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PenaltyPaymentId",
                table: "Attachments",
                column: "PenaltyPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPayments_PenaltyId",
                table: "PenaltyPayments",
                column: "PenaltyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Penalties_PenaltyId",
                table: "Attachments",
                column: "PenaltyId",
                principalTable: "Penalties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_PenaltyPayments_PenaltyPaymentId",
                table: "Attachments",
                column: "PenaltyPaymentId",
                principalTable: "PenaltyPayments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_YearOfOperations_YearOfOperationId",
                table: "Penalties",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Penalties_PenaltyId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_PenaltyPayments_PenaltyPaymentId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_YearOfOperations_YearOfOperationId",
                table: "Penalties");

            migrationBuilder.DropTable(
                name: "PenaltyPayments");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_YearOfOperationId",
                table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PenaltyId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PenaltyPaymentId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "PenaltyStatus",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "ResolutionComment",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "YearOfOperationId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "PenaltyId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PenaltyPaymentId",
                table: "Attachments");
        }
    }
}
