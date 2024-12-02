using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddNullCustomerIdfieldforInvoiceRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InvoiceRequests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests",
                column: "CustomerId",
                principalTable: "QBCustomers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InvoiceRequests",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                table: "InvoiceRequests",
                column: "CustomerId",
                principalTable: "QBCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
