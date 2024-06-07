using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedcommittebyaddingyearofoperationtothetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfOperationId",
                table: "Committees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Committees_YearOfOperationId",
                table: "Committees",
                column: "YearOfOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_YearOfOperations_YearOfOperationId",
                table: "Committees",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Committees_YearOfOperations_YearOfOperationId",
                table: "Committees");

            migrationBuilder.DropIndex(
                name: "IX_Committees_YearOfOperationId",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "YearOfOperationId",
                table: "Committees");
        }
    }
}
