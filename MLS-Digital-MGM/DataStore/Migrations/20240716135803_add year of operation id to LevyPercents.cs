using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addyearofoperationidtoLevyPercents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfOperationId",
                table: "LevyPercents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LevyPercents_YearOfOperationId",
                table: "LevyPercents",
                column: "YearOfOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LevyPercents_YearOfOperations_YearOfOperationId",
                table: "LevyPercents",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevyPercents_YearOfOperations_YearOfOperationId",
                table: "LevyPercents");

            migrationBuilder.DropIndex(
                name: "IX_LevyPercents_YearOfOperationId",
                table: "LevyPercents");

            migrationBuilder.DropColumn(
                name: "YearOfOperationId",
                table: "LevyPercents");
        }
    }
}
