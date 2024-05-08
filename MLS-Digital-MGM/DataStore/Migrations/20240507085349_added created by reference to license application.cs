using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatedbyreferencetolicenseapplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoValidTaxClearanceCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoSocietysLevy",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoProfessionalIndemnityInsuranceCover",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoMinimumNumberOfCLEUnits",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoFullMandatoryProBonoWork",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToMLSBuildingProjectFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToFidelityFund",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoAccountantsCertificate",
                table: "LicenseApplications",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "LicenseApplications",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplications_CreatedById",
                table: "LicenseApplications",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenseApplications_Users_CreatedById",
                table: "LicenseApplications",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenseApplications_Users_CreatedById",
                table: "LicenseApplications");

            migrationBuilder.DropIndex(
                name: "IX_LicenseApplications_CreatedById",
                table: "LicenseApplications");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "LicenseApplications");

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoValidTaxClearanceCertificate",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoSocietysLevy",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoProfessionalIndemnityInsuranceCover",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoMinimumNumberOfCLEUnits",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoFullMandatoryProBonoWork",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToMLSBuildingProjectFund",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoContributionToFidelityFund",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ExplanationForNoAccountantsCertificate",
                table: "LicenseApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);
        }
    }
}
