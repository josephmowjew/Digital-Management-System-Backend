using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddMadeContributiontoSocialWelfaretotheLicenseApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExplanationForNoContributionToSocialWelfare",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MadeContributionToSocialWelfare",
                table: "LicenseApplications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExplanationForNoContributionToSocialWelfare",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "MadeContributionToSocialWelfare",
                table: "LicenseApplications");
        }
    }
}
