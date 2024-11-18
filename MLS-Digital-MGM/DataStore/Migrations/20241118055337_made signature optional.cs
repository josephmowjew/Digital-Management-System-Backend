using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class madesignatureoptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Attachments_SubcommitteeMessages_SubcommitteeMessageId",
            //     table: "Attachments");

            // migrationBuilder.DropIndex(
            //     name: "IX_Attachments_SubcommitteeMessageId",
            //     table: "Attachments");

            // migrationBuilder.DropColumn(
            //     name: "SubcommitteeMessageId",
            //     table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "SignatureData",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            // migrationBuilder.CreateTable(
            //     name: "AttachmentSubcommitteeMessage",
            //     columns: table => new
            //     {
            //         AttachmentsId = table.Column<int>(type: "int", nullable: false),
            //         SubcommitteeMessagesId = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AttachmentSubcommitteeMessage", x => new { x.AttachmentsId, x.SubcommitteeMessagesId });
            //         table.ForeignKey(
            //             name: "FK_AttachmentSubcommitteeMessage_Attachments_AttachmentsId",
            //             column: x => x.AttachmentsId,
            //             principalTable: "Attachments",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_AttachmentSubcommitteeMessage_SubcommitteeMessages_Subcommit~",
            //             column: x => x.SubcommitteeMessagesId,
            //             principalTable: "SubcommitteeMessages",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     })
            //     .Annotation("MySQL:Charset", "utf8mb4");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AttachmentSubcommitteeMessage_SubcommitteeMessagesId",
            //     table: "AttachmentSubcommitteeMessage",
            //     column: "SubcommitteeMessagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentSubcommitteeMessage");

            migrationBuilder.AlterColumn<string>(
                name: "SignatureData",
                table: "Users",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcommitteeMessageId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_SubcommitteeMessageId",
                table: "Attachments",
                column: "SubcommitteeMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_SubcommitteeMessages_SubcommitteeMessageId",
                table: "Attachments",
                column: "SubcommitteeMessageId",
                principalTable: "SubcommitteeMessages",
                principalColumn: "Id");
        }
    }
}
