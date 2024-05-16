using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatedbyreferencetofirm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Firms",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Firms_CreatedById",
                table: "Firms",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_Users_CreatedById",
                table: "Firms",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_Users_CreatedById",
                table: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_Firms_CreatedById",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Firms");
        }
    }
}
