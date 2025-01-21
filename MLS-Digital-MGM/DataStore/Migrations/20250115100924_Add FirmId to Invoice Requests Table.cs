using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddFirmIdtoInvoiceRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "InvoiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_FirmId",
                table: "InvoiceRequests",
                column: "FirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_Firms_FirmId",
                table: "InvoiceRequests",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_Firms_FirmId",
                table: "InvoiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceRequests_FirmId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "InvoiceRequests");
        }
    }
}
