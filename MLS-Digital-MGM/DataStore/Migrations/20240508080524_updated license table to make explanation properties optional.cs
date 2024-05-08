using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class updatedlicensetabletomakeexplanationpropertiesoptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoValidTaxClearanceCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoSocietysLevy",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoProfessionalIndemnityInsuranceCover",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoFullMandatoryProBonoWork",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToMLSBuildingProjectFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToFidelityFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoComplianceWithPenalties",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoAccountantsCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "ExplanationForNoAnnualSubscriptionToSociety",
                table: "LicenseApplications",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExplanationForNoAnnualSubscriptionToSociety",
                table: "LicenseApplications");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoValidTaxClearanceCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoSocietysLevy",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoProfessionalIndemnityInsuranceCover",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoFullMandatoryProBonoWork",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToMLSBuildingProjectFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToFidelityFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoComplianceWithPenalties",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoAccountantsCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
