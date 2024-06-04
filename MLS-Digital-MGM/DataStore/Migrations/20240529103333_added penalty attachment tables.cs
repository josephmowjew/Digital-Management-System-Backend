using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedpenaltyattachmenttables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

          

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PenaltyId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PenaltyPaymentId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PenaltyId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PenaltyPaymentId",
                table: "Attachments");

            migrationBuilder.CreateTable(
                name: "AttachmentPenalty",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    PenaltiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentPenalty", x => new { x.AttachmentsId, x.PenaltiesId });
                    table.ForeignKey(
                        name: "FK_AttachmentPenalty_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentPenalty_Penalties_PenaltiesId",
                        column: x => x.PenaltiesId,
                        principalTable: "Penalties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentPenaltyPayment",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    PenaltyPaymentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentPenaltyPayment", x => new { x.AttachmentsId, x.PenaltyPaymentsId });
                    table.ForeignKey(
                        name: "FK_AttachmentPenaltyPayment_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentPenaltyPayment_PenaltyPayments_PenaltyPaymentsId",
                        column: x => x.PenaltyPaymentsId,
                        principalTable: "PenaltyPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentPenalty_PenaltiesId",
                table: "AttachmentPenalty",
                column: "PenaltiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentPenaltyPayment_PenaltyPaymentsId",
                table: "AttachmentPenaltyPayment",
                column: "PenaltyPaymentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentPenalty");

            migrationBuilder.DropTable(
                name: "AttachmentPenaltyPayment");

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

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PenaltyId",
                table: "Attachments",
                column: "PenaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PenaltyPaymentId",
                table: "Attachments",
                column: "PenaltyPaymentId");

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
        }
    }
}
