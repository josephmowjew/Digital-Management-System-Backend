using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedlicencemodeltomakelicenceapplicationIidnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicenseApplicationId",
                table: "Licenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Add the new foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_LicenseApplications_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId",
                principalTable: "LicenseApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_LicenseApplications_LicenseApplicationId",
                table: "Licenses");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseApplicationId",
                table: "Licenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_LicenseApplications_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId",
                principalTable: "LicenseApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
