using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addlicensetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserAttachment");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PermanentAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ResidentialAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DateOfAdmissionToPractice = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    IssuingInstitution = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DateObtained = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QualificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LicenseApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "longtext", nullable: false),
                    CurrentApprovalLevelID = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    FirstApplicationForLicense = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RenewedLicensePreviousYear = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObtainedLeaveToRenewLicenseOutOfTime = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PaidAnnualSubscriptionToSociety = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MadeContributionToFidelityFund = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoContributionToFidelityFund = table.Column<string>(type: "longtext", nullable: false),
                    RemittedSocietysLevy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoSocietysLevy = table.Column<string>(type: "longtext", nullable: false),
                    MadeContributionToMLSBuildingProjectFund = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoContributionToMLSBuildingProjectFund = table.Column<string>(type: "longtext", nullable: false),
                    PerformedFullMandatoryProBonoWork = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoFullMandatoryProBonoWork = table.Column<string>(type: "longtext", nullable: false),
                    AttainedMinimumNumberOfCLEUnits = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoMinimumNumberOfCLEUnits = table.Column<string>(type: "longtext", nullable: false),
                    HasValidAnnualProfessionalIndemnityInsuranceCover = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoProfessionalIndemnityInsuranceCover = table.Column<string>(type: "longtext", nullable: false),
                    SubmittedValidTaxClearanceCertificate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoValidTaxClearanceCertificate = table.Column<string>(type: "longtext", nullable: false),
                    SubmittedAccountantsCertificate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoAccountantsCertificate = table.Column<string>(type: "longtext", nullable: false),
                    CompliedWithPenaltiesImposedUnderTheAct = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoComplianceWithPenalties = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseApplications_LicenseApprovalLevels_CurrentApprovalLev~",
                        column: x => x.CurrentApprovalLevelID,
                        principalTable: "LicenseApprovalLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApplications_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApplications_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentQualification",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    QualificationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentQualification", x => new { x.AttachmentsId, x.QualificationsId });
                    table.ForeignKey(
                        name: "FK_AttachmentQualification_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentQualification_Qualifications_QualificationsId",
                        column: x => x.QualificationsId,
                        principalTable: "Qualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberQualification",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    QualificationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberQualification", x => new { x.MembersId, x.QualificationsId });
                    table.ForeignKey(
                        name: "FK_MemberQualification_Members_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberQualification_Qualifications_QualificationsId",
                        column: x => x.QualificationsId,
                        principalTable: "Qualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentLicenseApplication",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    LicenseApplicationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentLicenseApplication", x => new { x.AttachmentsId, x.LicenseApplicationsId });
                    table.ForeignKey(
                        name: "FK_AttachmentLicenseApplication_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentLicenseApplication_LicenseApplications_LicenseAppl~",
                        column: x => x.LicenseApplicationsId,
                        principalTable: "LicenseApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LicenseApplicationApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LicenseApplicationID = table.Column<int>(type: "int", nullable: false),
                    LicenseApprovalLevelID = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Reason_for_Rejection = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseApplicationApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseApplicationApprovals_LicenseApplications_LicenseAppli~",
                        column: x => x.LicenseApplicationID,
                        principalTable: "LicenseApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApplicationApprovals_LicenseApprovalLevels_LicenseApp~",
                        column: x => x.LicenseApprovalLevelID,
                        principalTable: "LicenseApprovalLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApplicationApprovals_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LicenseNumber = table.Column<string>(type: "longtext", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    LicenseApplicationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_LicenseApplications_LicenseApplicationId",
                        column: x => x.LicenseApplicationId,
                        principalTable: "LicenseApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Licenses_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Licenses_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentLicenseApplication_LicenseApplicationsId",
                table: "AttachmentLicenseApplication",
                column: "LicenseApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentQualification_QualificationsId",
                table: "AttachmentQualification",
                column: "QualificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplicationApprovals_CreatedById",
                table: "LicenseApplicationApprovals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplicationApprovals_LicenseApplicationID",
                table: "LicenseApplicationApprovals",
                column: "LicenseApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplicationApprovals_LicenseApprovalLevelID",
                table: "LicenseApplicationApprovals",
                column: "LicenseApprovalLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplications_CurrentApprovalLevelID",
                table: "LicenseApplications",
                column: "CurrentApprovalLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplications_MemberId",
                table: "LicenseApplications",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApplications_YearOfOperationId",
                table: "LicenseApplications",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_MemberId",
                table: "Licenses",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_YearOfOperationId",
                table: "Licenses",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberQualification_QualificationsId",
                table: "MemberQualification",
                column: "QualificationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentLicenseApplication");

            migrationBuilder.DropTable(
                name: "AttachmentQualification");

            migrationBuilder.DropTable(
                name: "LicenseApplicationApprovals");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "MemberQualification");

            migrationBuilder.DropTable(
                name: "QualificationTypes");

            migrationBuilder.DropTable(
                name: "LicenseApplications");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.CreateTable(
                name: "ApplicationUserAttachment",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserAttachment", x => new { x.AttachmentsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserAttachment_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserAttachment_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserAttachment_UsersId",
                table: "ApplicationUserAttachment",
                column: "UsersId");
        }
    }
}
