using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedroletocommittemembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_CommitteeMembers_Members_MemberID",
            //     table: "CommitteeMembers");

            // migrationBuilder.DropIndex(
            //     name: "IX_CommitteeMembers_MemberID",
            //     table: "CommitteeMembers");

            migrationBuilder.DropColumn(
                name: "MemberID",
                table: "CommitteeMembers");

            migrationBuilder.AddColumn<string>(
                name: "MemberShipId",
                table: "CommitteeMembers",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CommitteeMembers",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMembers_MemberShipId",
                table: "CommitteeMembers",
                column: "MemberShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommitteeMembers_Users_MemberShipId",
                table: "CommitteeMembers",
                column: "MemberShipId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeMembers_Users_MemberShipId",
                table: "CommitteeMembers");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeMembers_MemberShipId",
                table: "CommitteeMembers");

            migrationBuilder.DropColumn(
                name: "MemberShipId",
                table: "CommitteeMembers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CommitteeMembers");

            migrationBuilder.AddColumn<int>(
                name: "MemberID",
                table: "CommitteeMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMembers_MemberID",
                table: "CommitteeMembers",
                column: "MemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_CommitteeMembers_Members_MemberID",
                table: "CommitteeMembers",
                column: "MemberID",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
