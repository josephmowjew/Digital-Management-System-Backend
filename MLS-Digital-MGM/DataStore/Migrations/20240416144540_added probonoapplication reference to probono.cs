using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedprobonoapplicationreferencetoprobono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProBonoApplicationId",
                table: "ProBono",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProBono_ProBonoApplicationId",
                table: "ProBono",
                column: "ProBonoApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProBono_ProBonoApplications_ProBonoApplicationId",
                table: "ProBono",
                column: "ProBonoApplicationId",
                principalTable: "ProBonoApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProBono_ProBonoApplications_ProBonoApplicationId",
                table: "ProBono");

            migrationBuilder.DropIndex(
                name: "IX_ProBono_ProBonoApplicationId",
                table: "ProBono");

            migrationBuilder.DropColumn(
                name: "ProBonoApplicationId",
                table: "ProBono");
        }
    }
}
