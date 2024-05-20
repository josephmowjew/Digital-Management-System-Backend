using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedlicenseexpirydatedatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Licenses",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ExpiryDate",
                table: "Licenses",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId");
        }
    }
}
