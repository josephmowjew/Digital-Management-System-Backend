using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedmanytomanybetweencpdtrainingregistrationandattachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_CPDTrainingRegistrations_AttachmentId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.CreateTable(
                name: "AttachmentCPDTrainingRegistration",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    CPDTrainingRegistrationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCPDTrainingRegistration", x => new { x.AttachmentsId, x.CPDTrainingRegistrationsId });
                    table.ForeignKey(
                        name: "FK_AttachmentCPDTrainingRegistration_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentCPDTrainingRegistration_CPDTrainingRegistrations_C~",
                        column: x => x.CPDTrainingRegistrationsId,
                        principalTable: "CPDTrainingRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCPDTrainingRegistration_CPDTrainingRegistrationsId",
                table: "AttachmentCPDTrainingRegistration",
                column: "CPDTrainingRegistrationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentCPDTrainingRegistration");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "CPDTrainingRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_AttachmentId",
                table: "CPDTrainingRegistrations",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_Attachments_AttachmentId",
                table: "CPDTrainingRegistrations",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }
    }
}
