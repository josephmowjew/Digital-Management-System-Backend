using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addeddescriptiontoinvoicerequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InvoiceRequests",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InvoiceRequests",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QBInvoiceId",
                table: "InvoiceRequests",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_CustomerId",
                table: "InvoiceRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_QBInvoiceId",
                table: "InvoiceRequests",
                column: "QBInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests",
                column: "CustomerId",
                principalTable: "QBCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_QBInvoices_QBInvoiceId",
                table: "InvoiceRequests",
                column: "QBInvoiceId",
                principalTable: "QBInvoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_QBInvoices_QBInvoiceId",
                table: "InvoiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceRequests_CustomerId",
                table: "InvoiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceRequests_QBInvoiceId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "QBInvoiceId",
                table: "InvoiceRequests");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InvoiceRequests",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
