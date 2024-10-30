using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class UserIdtothenotariestale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NotariesPublic",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NotariesPublic_UserId",
                table: "NotariesPublic",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotariesPublic_Users_UserId",
                table: "NotariesPublic",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotariesPublic_Users_UserId",
                table: "NotariesPublic");

            migrationBuilder.DropIndex(
                name: "IX_NotariesPublic_UserId",
                table: "NotariesPublic");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotariesPublic");
        }
    }
}
