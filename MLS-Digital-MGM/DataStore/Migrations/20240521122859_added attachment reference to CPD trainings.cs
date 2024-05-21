using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedattachmentreferencetoCPDtrainings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "CPDTrainings",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "AttachmentCPDTraining",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    CPDTrainingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCPDTraining", x => new { x.AttachmentsId, x.CPDTrainingsId });
                    table.ForeignKey(
                        name: "FK_AttachmentCPDTraining_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentCPDTraining_CPDTrainings_CPDTrainingsId",
                        column: x => x.CPDTrainingsId,
                        principalTable: "CPDTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCPDTraining_CPDTrainingsId",
                table: "AttachmentCPDTraining",
                column: "CPDTrainingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentCPDTraining");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "CPDTrainings");
        }
    }
}
