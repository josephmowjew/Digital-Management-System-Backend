using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class Addattachmentstoinvoicerequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "InvoiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "InvoiceRequests",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_AttachmentId",
                table: "InvoiceRequests",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_Attachments_AttachmentId",
                table: "InvoiceRequests",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_Attachments_AttachmentId",
                table: "InvoiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceRequests_AttachmentId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "InvoiceRequests");
        }
    }
}
