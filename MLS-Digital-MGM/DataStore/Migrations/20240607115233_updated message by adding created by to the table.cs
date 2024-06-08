using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedmessagebyaddingcreatedbytothetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Messages_Members_SenderID",
            //     table: "Messages");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_Messages_Threads_ThreadID",
            //     table: "Messages");

            // migrationBuilder.DropIndex(
            //     name: "IX_Messages_SenderID",
            //     table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "Messages");

           
             migrationBuilder.Sql("ALTER TABLE Messages RENAME COLUMN ThreadID TO ThreadId;");
            migrationBuilder.RenameIndex(
                name: "IX_Messages_ThreadID",
                table: "Messages",
                newName: "IX_Messages_ThreadId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Messages",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedById",
                table: "Messages",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Threads_ThreadId",
                table: "Messages",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Threads_ThreadId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_CreatedById",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "Messages",
                newName: "ThreadID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ThreadId",
                table: "Messages",
                newName: "IX_Messages_ThreadID");

            migrationBuilder.AddColumn<int>(
                name: "SenderID",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Members_SenderID",
                table: "Messages",
                column: "SenderID",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Threads_ThreadID",
                table: "Messages",
                column: "ThreadID",
                principalTable: "Threads",
                principalColumn: "Id");
        }
    }
}
