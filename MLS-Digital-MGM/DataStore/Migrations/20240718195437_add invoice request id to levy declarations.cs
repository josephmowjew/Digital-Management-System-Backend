using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addinvoicerequestidtolevydeclarations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceRequestId",
                table: "LevyDeclarations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevyDeclarations_InvoiceRequestId",
                table: "LevyDeclarations",
                column: "InvoiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_LevyDeclarations_InvoiceRequests_InvoiceRequestId",
                table: "LevyDeclarations",
                column: "InvoiceRequestId",
                principalTable: "InvoiceRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevyDeclarations_InvoiceRequests_InvoiceRequestId",
                table: "LevyDeclarations");

            migrationBuilder.DropIndex(
                name: "IX_LevyDeclarations_InvoiceRequestId",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestId",
                table: "LevyDeclarations");
        }
    }
}
