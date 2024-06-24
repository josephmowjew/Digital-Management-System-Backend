using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addingcertificateofadmissionandexplanationfornotsubmittingtolicenseapplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CertificateOfAdmission",
                table: "LicenseApplications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExplanationForNotSubmittingCertificateOfAdmission",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateOfAdmission",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "ExplanationForNotSubmittingCertificateOfAdmission",
                table: "LicenseApplications");
        }
    }
}
