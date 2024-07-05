using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQBinvoiceidfrompenalties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Penalties_QBInvoices_QBInvoiceId",
            //     table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_QBInvoiceId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "QBInvoiceId",
                table: "Penalties");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceRequestId",
                table: "Penalties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_InvoiceRequestId",
                table: "Penalties",
                column: "InvoiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_InvoiceRequests_InvoiceRequestId",
                table: "Penalties",
                column: "InvoiceRequestId",
                principalTable: "InvoiceRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_InvoiceRequests_InvoiceRequestId",
                table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_InvoiceRequestId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestId",
                table: "Penalties");

            migrationBuilder.AddColumn<string>(
                name: "QBInvoiceId",
                table: "Penalties",
                type: "varchar(255)",
                nullable: true);

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
        }
    }
}
