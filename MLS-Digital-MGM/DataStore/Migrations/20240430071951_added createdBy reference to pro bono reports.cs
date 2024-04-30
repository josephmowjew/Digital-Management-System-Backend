using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatedByreferencetoprobonoreports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProBonoReports",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_CreatedById",
                table: "ProBonoReports",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ProBonoReports_Users_CreatedById",
                table: "ProBonoReports",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProBonoReports_Users_CreatedById",
                table: "ProBonoReports");

            migrationBuilder.DropIndex(
                name: "IX_ProBonoReports_CreatedById",
                table: "ProBonoReports");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProBonoReports");
        }
    }
}
