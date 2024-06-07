using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedmessagingmodules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingSchedule",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CommitteeMembers");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Committees",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_CreatedById",
                table: "Committees",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Users_CreatedById",
                table: "Committees",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Committees_Users_CreatedById",
                table: "Committees");

            migrationBuilder.DropIndex(
                name: "IX_Committees_CreatedById",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Committees");

            migrationBuilder.AddColumn<string>(
                name: "MeetingSchedule",
                table: "Committees",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CommitteeMembers",
                type: "longtext",
                nullable: false);
        }
    }
}
