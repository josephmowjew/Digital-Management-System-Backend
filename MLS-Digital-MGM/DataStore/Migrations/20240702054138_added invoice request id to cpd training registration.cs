using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedinvoicerequestidtocpdtrainingregistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceRequestId",
                table: "CPDTrainingRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_InvoiceRequestId",
                table: "CPDTrainingRegistrations",
                column: "InvoiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_InvoiceRequests_InvoiceRequestId",
                table: "CPDTrainingRegistrations",
                column: "InvoiceRequestId",
                principalTable: "InvoiceRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDTrainingRegistrations_InvoiceRequests_InvoiceRequestId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_CPDTrainingRegistrations_InvoiceRequestId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestId",
                table: "CPDTrainingRegistrations");
        }
    }
}
