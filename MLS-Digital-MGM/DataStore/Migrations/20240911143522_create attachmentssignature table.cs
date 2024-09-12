using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class createattachmentssignaturetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Signatures_SignatureId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_SignatureId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "Attachments");

            migrationBuilder.CreateTable(
                name: "AttachmentSignature",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    SignaturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentSignature", x => new { x.AttachmentsId, x.SignaturesId });
                    table.ForeignKey(
                        name: "FK_AttachmentSignature_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentSignature_Signatures_SignaturesId",
                        column: x => x.SignaturesId,
                        principalTable: "Signatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentSignature_SignaturesId",
                table: "AttachmentSignature",
                column: "SignaturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentSignature");

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_SignatureId",
                table: "Attachments",
                column: "SignatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Signatures_SignatureId",
                table: "Attachments",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id");
        }
    }
}
