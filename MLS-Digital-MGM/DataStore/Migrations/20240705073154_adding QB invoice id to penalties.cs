using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addingQBinvoiceidtopenalties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QBInvoiceId",
                table: "PenaltyPayments",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QBInvoiceId",
                table: "Penalties",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPayments_QBInvoiceId",
                table: "PenaltyPayments",
                column: "QBInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_QBInvoiceId",
                table: "Penalties",
                column: "QBInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_QBInvoices_QBInvoiceId",
                table: "Penalties",
                column: "QBInvoiceId",
                principalTable: "QBInvoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PenaltyPayments_QBInvoices_QBInvoiceId",
                table: "PenaltyPayments",
                column: "QBInvoiceId",
                principalTable: "QBInvoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_QBInvoices_QBInvoiceId",
                table: "Penalties");

            migrationBuilder.DropForeignKey(
                name: "FK_PenaltyPayments_QBInvoices_QBInvoiceId",
                table: "PenaltyPayments");

            migrationBuilder.DropIndex(
                name: "IX_PenaltyPayments_QBInvoiceId",
                table: "PenaltyPayments");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_QBInvoiceId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "QBInvoiceId",
                table: "PenaltyPayments");

            migrationBuilder.DropColumn(
                name: "QBInvoiceId",
                table: "Penalties");
        }
    }
}
