using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePicturestotheApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserAttachment",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "varchar(200)", nullable: false),
                    ProfilePicturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserAttachment", x => new { x.ApplicationUsersId, x.ProfilePicturesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserAttachment_Attachments_ProfilePicturesId",
                        column: x => x.ProfilePicturesId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserAttachment_Users_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserAttachment_ProfilePicturesId",
                table: "ApplicationUserAttachment",
                column: "ProfilePicturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserAttachment");
        }
    }
}
