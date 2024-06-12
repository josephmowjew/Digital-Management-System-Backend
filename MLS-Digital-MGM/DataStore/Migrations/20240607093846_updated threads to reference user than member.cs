using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedthreadstoreferenceuserthanmember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Threads_Committees_CommitteeID",
            //     table: "Threads");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_Threads_Members_CreatedByMemberId",
            //     table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_CreatedByMemberId",
                table: "Threads");

           migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "CreatedByMemberId",
                table: "Threads");

          
                
            migrationBuilder.Sql("ALTER TABLE Threads RENAME COLUMN CommitteeID TO CommitteeId;");
            migrationBuilder.RenameIndex(
                name: "IX_Threads_CommitteeID",
                table: "Threads",
                newName: "IX_Threads_CommitteeId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Threads",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CreatedById",
                table: "Threads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_PenaltyTypeId",
                table: "Penalties",
                column: "PenaltyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_PenaltyTypes_PenaltyTypeId",
                table: "Penalties",
                column: "PenaltyTypeId",
                principalTable: "PenaltyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Committees_CommitteeId",
                table: "Threads",
                column: "CommitteeId",
                principalTable: "Committees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Users_CreatedById",
                table: "Threads",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_PenaltyTypes_PenaltyTypeId",
                table: "Penalties");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Committees_CommitteeId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Users_CreatedById",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_CreatedById",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_PenaltyTypeId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "CommitteeId",
                table: "Threads",
                newName: "CommitteeID");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_CommitteeId",
                table: "Threads",
                newName: "IX_Threads_CommitteeID");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Threads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByMemberId",
                table: "Threads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CreatedByMemberId",
                table: "Threads",
                column: "CreatedByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Committees_CommitteeID",
                table: "Threads",
                column: "CommitteeID",
                principalTable: "Committees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Members_CreatedByMemberId",
                table: "Threads",
                column: "CreatedByMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
