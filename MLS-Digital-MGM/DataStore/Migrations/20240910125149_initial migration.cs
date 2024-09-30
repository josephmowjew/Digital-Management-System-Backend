using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ShortCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdentityTypes",
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
                    table.PrimaryKey("PK_IdentityTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PenaltyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QBCustomers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(150)", nullable: false),
                    CustomerName = table.Column<string>(type: "longtext", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    MiddleName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    JobTitle = table.Column<string>(type: "longtext", nullable: false),
                    CompanyName = table.Column<string>(type: "longtext", nullable: false),
                    BillingAddressLine1 = table.Column<string>(type: "longtext", nullable: false),
                    BillingAddressLine2 = table.Column<string>(type: "longtext", nullable: false),
                    BillingAddressLine3 = table.Column<string>(type: "longtext", nullable: false),
                    BillingAddressLine4 = table.Column<string>(type: "longtext", nullable: false),
                    BillingAddressLine5 = table.Column<string>(type: "longtext", nullable: false),
                    City = table.Column<string>(type: "longtext", nullable: false),
                    State = table.Column<string>(type: "longtext", nullable: false),
                    Province = table.Column<string>(type: "longtext", nullable: false),
                    ActiveStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmailAddress = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QBCustomers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QualificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    Discriminator = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "YearOfOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearOfOperations", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LicenseApprovalLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseApprovalLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalLevels_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QBInvoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(150)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "longtext", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    CustomerName = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnpaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceType = table.Column<string>(type: "longtext", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InvoiceDescription = table.Column<string>(type: "longtext", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QBInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QBInvoices_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevyPercents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PercentageValue = table.Column<double>(type: "double", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    OperationStatus = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevyPercents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevyPercents_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stamps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stamps_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QBPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(150)", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    InvoiceId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QBPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QBPayments_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QBPayments_QBInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "QBInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QBReceipts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(150)", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "longtext", nullable: false),
                    TotalPaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentId = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceId = table.Column<string>(type: "varchar(255)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QBReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QBReceipts_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QBReceipts_QBInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "QBInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentCPDTraining",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    CPDTrainingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCPDTraining", x => new { x.AttachmentsId, x.CPDTrainingsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentCPDTrainingRegistration",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    CPDTrainingRegistrationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCPDTrainingRegistration", x => new { x.AttachmentsId, x.CPDTrainingRegistrationsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentLevyDeclaration",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    LevyDeclarationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentLevyDeclaration", x => new { x.AttachmentsId, x.LevyDeclarationsId });
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentMemberQualification",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    MemberQualificationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentMemberQualification", x => new { x.AttachmentsId, x.MemberQualificationsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentMessage",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    MessagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentMessage", x => new { x.AttachmentsId, x.MessagesId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentPenalty",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    PenaltiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentPenalty", x => new { x.AttachmentsId, x.PenaltiesId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentPenaltyPayment",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    PenaltyPaymentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentPenaltyPayment", x => new { x.AttachmentsId, x.PenaltyPaymentsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentProBono",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    ProBonosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentProBono", x => new { x.AttachmentsId, x.ProBonosId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentProBonoApplication",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    ProBonosApplicationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentProBonoApplication", x => new { x.AttachmentsId, x.ProBonosApplicationsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentProBonoReport",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    ProBonoReportsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentProBonoReport", x => new { x.AttachmentsId, x.ProBonoReportsId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PropertyName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
                    SubcommitteeMessageId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_AttachmentTypes_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentStamp",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    StampsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentStamp", x => new { x.AttachmentsId, x.StampsId });
                    table.ForeignKey(
                        name: "FK_AttachmentStamp_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentStamp_Stamps_StampsId",
                        column: x => x.StampsId,
                        principalTable: "Stamps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommitteeMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CommitteeID = table.Column<int>(type: "int", nullable: false),
                    MemberShipId = table.Column<string>(type: "varchar(200)", nullable: false),
                    MemberShipStatus = table.Column<string>(type: "longtext", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeMembers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CommitteeName = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ChairpersonID = table.Column<int>(type: "int", nullable: true),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Committees_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommunicationMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(type: "longtext", nullable: false),
                    Body = table.Column<string>(type: "longtext", nullable: false),
                    SentByUserId = table.Column<string>(type: "varchar(200)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    SentToAllUsers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TargetedRolesJson = table.Column<string>(type: "longtext", nullable: false),
                    TargetedDepartmentsJson = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationMessages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CPDTrainingRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatus = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CPDTrainingId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    DeniedReason = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DateOfPayment = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InvoiceRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPDTrainingRegistrations", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CPDTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Duration = table.Column<double>(type: "double", nullable: false),
                    DateToBeConducted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhysicalVenue = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    ApprovalStatus = table.Column<string>(type: "longtext", nullable: false),
                    ProposedUnits = table.Column<int>(type: "int", nullable: false),
                    MemberPhysicalAttendanceFee = table.Column<double>(type: "double", nullable: true),
                    MemberVirtualAttendanceFee = table.Column<double>(type: "double", nullable: true),
                    NonMemberPhysicalAttendanceFee = table.Column<double>(type: "double", nullable: true),
                    NonMemberVirtualAttandanceFee = table.Column<double>(type: "double", nullable: true),
                    RegistrationDueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsFree = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CPDUnitsAwarded = table.Column<int>(type: "int", nullable: false),
                    AccreditingInstitution = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPDTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPDTrainings_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CPDUnitsEarned",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CPDTrainingId = table.Column<int>(type: "int", nullable: false),
                    UnitsEarned = table.Column<int>(type: "int", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPDUnitsEarned", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPDUnitsEarned_CPDTrainings_CPDTrainingId",
                        column: x => x.CPDTrainingId,
                        principalTable: "CPDTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPDUnitsEarned_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserFriendlyMessage = table.Column<string>(type: "longtext", nullable: false),
                    DetailedMessage = table.Column<string>(type: "longtext", nullable: false),
                    DateOccurred = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Firms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PhysicalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PrimaryContactPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PrimaryPhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    SecondaryContactPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    SecondaryPhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    DenialReason = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Firms_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    OtherName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IdentityExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IdentityTypeId = table.Column<int>(type: "int", nullable: false),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Firms_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_IdentityTypes_IdentityTypeId",
                        column: x => x.IdentityTypeId,
                        principalTable: "IdentityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InvoiceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    ReferencedEntityType = table.Column<string>(type: "longtext", nullable: false),
                    ReferencedEntityId = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    QBInvoiceId = table.Column<string>(type: "varchar(255)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_QBInvoices_QBInvoiceId",
                        column: x => x.QBInvoiceId,
                        principalTable: "QBInvoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(200)", nullable: false),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PermanentAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ResidentialAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DateOfAdmissionToPractice = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Firms_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Members_QBCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "QBCustomers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProbonoClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NationalId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PermanentAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ResidentialAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    OtherContacts = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Occupation = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    deleteRequest = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProbonoClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProbonoClients_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threads_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Threads_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    UserId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    RoleId = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false),
                    Value = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevyDeclarations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Month = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LevyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FirmId = table.Column<int>(type: "int", nullable: false),
                    InvoiceRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevyDeclarations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevyDeclarations_Firms_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LevyDeclarations_InvoiceRequests_InvoiceRequestId",
                        column: x => x.InvoiceRequestId,
                        principalTable: "InvoiceRequests",
                        principalColumn: "Id");
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
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    FirstApplicationForLicense = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RenewedLicensePreviousYear = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObtainedLeaveToRenewLicenseOutOfTime = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PaidAnnualSubscriptionToSociety = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoAnnualSubscriptionToSociety = table.Column<string>(type: "longtext", nullable: true),
                    MadeContributionToFidelityFund = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoContributionToFidelityFund = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    RemittedSocietysLevy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoSocietysLevy = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    MadeContributionToMLSBuildingProjectFund = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoContributionToMLSBuildingProjectFund = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PerformedFullMandatoryProBonoWork = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoFullMandatoryProBonoWork = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    AttainedMinimumNumberOfCLEUnits = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoMinimumNumberOfCLEUnits = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    HasValidAnnualProfessionalIndemnityInsuranceCover = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoProfessionalIndemnityInsuranceCover = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    SubmittedValidTaxClearanceCertificate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoValidTaxClearanceCertificate = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    SubmittedAccountantsCertificate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoAccountantsCertificate = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CompliedWithPenaltiesImposedUnderTheAct = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNoComplianceWithPenalties = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CertificateOfAdmission = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExplanationForNotSubmittingCertificateOfAdmission = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
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
                        name: "FK_LicenseApplications_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
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
                name: "MemberQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    IssuingInstitution = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DateObtained = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    QualificationTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberQualifications_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberQualifications_QualificationTypes_QualificationTypeId",
                        column: x => x.QualificationTypeId,
                        principalTable: "QualificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberQualificationType",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    QualificationTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberQualificationType", x => new { x.MembersId, x.QualificationTypesId });
                    table.ForeignKey(
                        name: "FK_MemberQualificationType_Members_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberQualificationType_QualificationTypes_QualificationType~",
                        column: x => x.QualificationTypesId,
                        principalTable: "QualificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    PenaltyTypeId = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<double>(type: "double", nullable: false),
                    AmountRemaining = table.Column<double>(type: "double", nullable: false),
                    PenaltyStatus = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ResolutionComment = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    InvoiceRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalties_InvoiceRequests_InvoiceRequestId",
                        column: x => x.InvoiceRequestId,
                        principalTable: "InvoiceRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Penalties_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Penalties_PenaltyTypes_PenaltyTypeId",
                        column: x => x.PenaltyTypeId,
                        principalTable: "PenaltyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Penalties_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Penalties_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subcommittees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubcommitteeName = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    ChairpersonId = table.Column<int>(type: "int", nullable: true),
                    CommitteeId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcommittees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcommittees_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subcommittees_Members_ChairpersonId",
                        column: x => x.ChairpersonId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subcommittees_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProBonoApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NatureOfDispute = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CaseDetails = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    ProbonoClientId = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "longtext", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DenialReason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    SummaryOfDispute = table.Column<string>(type: "longtext", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProBonoApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProBonoApplications_ProbonoClients_ProbonoClientId",
                        column: x => x.ProbonoClientId,
                        principalTable: "ProbonoClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBonoApplications_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProBonoApplications_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CommitteeID = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    ThreadId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Committees_CommitteeID",
                        column: x => x.CommitteeID,
                        principalTable: "Committees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
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
                name: "LicenseApprovalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LicenseApplicationId = table.Column<int>(type: "int", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovalLevelId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    ChangedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseApprovalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalHistories_LicenseApplications_LicenseApplicat~",
                        column: x => x.LicenseApplicationId,
                        principalTable: "LicenseApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalHistories_LicenseApprovalLevels_ApprovalLevel~",
                        column: x => x.ApprovalLevelId,
                        principalTable: "LicenseApprovalLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalHistories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "PenaltyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PenaltyId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    DateApproved = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateDenied = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReasonForDenial = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    QBInvoiceId = table.Column<string>(type: "varchar(255)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PenaltyPayments_Penalties_PenaltyId",
                        column: x => x.PenaltyId,
                        principalTable: "Penalties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PenaltyPayments_QBInvoices_QBInvoiceId",
                        column: x => x.QBInvoiceId,
                        principalTable: "QBInvoices",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubcommitteeMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubcommitteeID = table.Column<int>(type: "int", nullable: false),
                    MemberShipId = table.Column<string>(type: "varchar(200)", nullable: false),
                    MemberShipStatus = table.Column<string>(type: "longtext", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcommitteeMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubcommitteeMemberships_Subcommittees_SubcommitteeID",
                        column: x => x.SubcommitteeID,
                        principalTable: "Subcommittees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubcommitteeMemberships_Users_MemberShipId",
                        column: x => x.MemberShipId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubcommitteeThreads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubcommitteeId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcommitteeThreads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubcommitteeThreads_Subcommittees_SubcommitteeId",
                        column: x => x.SubcommitteeId,
                        principalTable: "Subcommittees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubcommitteeThreads_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProBonos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FileNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NatureOfDispute = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CaseDetails = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    ProbonoClientId = table.Column<int>(type: "int", nullable: false),
                    ProBonoApplicationId = table.Column<int>(type: "int", nullable: false),
                    SummaryOfDispute = table.Column<string>(type: "longtext", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProBonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProBonos_ProBonoApplications_ProBonoApplicationId",
                        column: x => x.ProBonoApplicationId,
                        principalTable: "ProBonoApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBonos_ProbonoClients_ProbonoClientId",
                        column: x => x.ProbonoClientId,
                        principalTable: "ProbonoClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBonos_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProBonos_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LicenseApprovalComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ApprovalHistoryId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CommentedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseApprovalComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalComments_LicenseApprovalHistories_ApprovalHis~",
                        column: x => x.ApprovalHistoryId,
                        principalTable: "LicenseApprovalHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseApprovalComments_Users_CommentedById",
                        column: x => x.CommentedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubcommitteeMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubcommitteeID = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    SubcommitteeThreadId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcommitteeMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubcommitteeMessages_SubcommitteeThreads_SubcommitteeThreadId",
                        column: x => x.SubcommitteeThreadId,
                        principalTable: "SubcommitteeThreads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubcommitteeMessages_Subcommittees_SubcommitteeID",
                        column: x => x.SubcommitteeID,
                        principalTable: "Subcommittees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubcommitteeMessages_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberProBono",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    ProBonosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProBono", x => new { x.MembersId, x.ProBonosId });
                    table.ForeignKey(
                        name: "FK_MemberProBono_Members_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberProBono_ProBonos_ProBonosId",
                        column: x => x.ProBonosId,
                        principalTable: "ProBonos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProBonoReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProBonoId = table.Column<int>(type: "int", nullable: false),
                    ProBonoProposedHours = table.Column<double>(type: "double", nullable: false),
                    ProBonoHours = table.Column<double>(type: "double", nullable: false),
                    ReportStatus = table.Column<string>(type: "longtext", nullable: false),
                    ApprovedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProBonoReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProBonoReports_ProBonos_ProBonoId",
                        column: x => x.ProBonoId,
                        principalTable: "ProBonos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBonoReports_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProBonoReports_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropBonoReportFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProBonoReportId = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    FeedBackById = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropBonoReportFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropBonoReportFeedbacks_ProBonoReports_ProBonoReportId",
                        column: x => x.ProBonoReportId,
                        principalTable: "ProBonoReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropBonoReportFeedbacks_Users_FeedBackById",
                        column: x => x.FeedBackById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCPDTraining_CPDTrainingsId",
                table: "AttachmentCPDTraining",
                column: "CPDTrainingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCPDTrainingRegistration_CPDTrainingRegistrationsId",
                table: "AttachmentCPDTrainingRegistration",
                column: "CPDTrainingRegistrationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentLevyDeclaration_LevyDeclarationsId",
                table: "AttachmentLevyDeclaration",
                column: "LevyDeclarationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentLicenseApplication_LicenseApplicationsId",
                table: "AttachmentLicenseApplication",
                column: "LicenseApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentMemberQualification_MemberQualificationsId",
                table: "AttachmentMemberQualification",
                column: "MemberQualificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentMessage_MessagesId",
                table: "AttachmentMessage",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentPenalty_PenaltiesId",
                table: "AttachmentPenalty",
                column: "PenaltiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentPenaltyPayment_PenaltyPaymentsId",
                table: "AttachmentPenaltyPayment",
                column: "PenaltyPaymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentProBono_ProBonosId",
                table: "AttachmentProBono",
                column: "ProBonosId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentProBonoApplication_ProBonosApplicationsId",
                table: "AttachmentProBonoApplication",
                column: "ProBonosApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentProBonoReport_ProBonoReportsId",
                table: "AttachmentProBonoReport",
                column: "ProBonoReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AttachmentTypeId",
                table: "Attachments",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_SubcommitteeMessageId",
                table: "Attachments",
                column: "SubcommitteeMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentStamp_StampsId",
                table: "AttachmentStamp",
                column: "StampsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMembers_CommitteeID",
                table: "CommitteeMembers",
                column: "CommitteeID");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMembers_MemberShipId",
                table: "CommitteeMembers",
                column: "MemberShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ChairpersonID",
                table: "Committees",
                column: "ChairpersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_CreatedById",
                table: "Committees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_YearOfOperationId",
                table: "Committees",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_SentByUserId",
                table: "CommunicationMessages",
                column: "SentByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_CPDTrainingId",
                table: "CPDTrainingRegistrations",
                column: "CPDTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_CreatedById",
                table: "CPDTrainingRegistrations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_InvoiceRequestId",
                table: "CPDTrainingRegistrations",
                column: "InvoiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_MemberId",
                table: "CPDTrainingRegistrations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainings_CreatedById",
                table: "CPDTrainings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainings_YearOfOperationId",
                table: "CPDTrainings",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDUnitsEarned_CPDTrainingId",
                table: "CPDUnitsEarned",
                column: "CPDTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDUnitsEarned_MemberId",
                table: "CPDUnitsEarned",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDUnitsEarned_YearOfOperationId",
                table: "CPDUnitsEarned",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_CreatedById",
                table: "ErrorLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Firms_CreatedById",
                table: "Firms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Firms_CustomerId",
                table: "Firms",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_CreatedById",
                table: "InvoiceRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_CustomerId",
                table: "InvoiceRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_QBInvoiceId",
                table: "InvoiceRequests",
                column: "QBInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_YearOfOperationId",
                table: "InvoiceRequests",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_LevyDeclarations_FirmId",
                table: "LevyDeclarations",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_LevyDeclarations_InvoiceRequestId",
                table: "LevyDeclarations",
                column: "InvoiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LevyPercents_YearOfOperationId",
                table: "LevyPercents",
                column: "YearOfOperationId");

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
                name: "IX_LicenseApplications_CreatedById",
                table: "LicenseApplications",
                column: "CreatedById");

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
                name: "IX_LicenseApprovalComments_ApprovalHistoryId",
                table: "LicenseApprovalComments",
                column: "ApprovalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalComments_CommentedById",
                table: "LicenseApprovalComments",
                column: "CommentedById");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalHistories_ApprovalLevelId",
                table: "LicenseApprovalHistories",
                column: "ApprovalLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalHistories_ChangedById",
                table: "LicenseApprovalHistories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalHistories_LicenseApplicationId",
                table: "LicenseApprovalHistories",
                column: "LicenseApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseApprovalLevels_DepartmentId",
                table: "LicenseApprovalLevels",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseApplicationId",
                table: "Licenses",
                column: "LicenseApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_MemberId",
                table: "Licenses",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_YearOfOperationId",
                table: "Licenses",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProBono_ProBonosId",
                table: "MemberProBono",
                column: "ProBonosId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberQualifications_MemberId",
                table: "MemberQualifications",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberQualifications_QualificationTypeId",
                table: "MemberQualifications",
                column: "QualificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberQualificationType_QualificationTypesId",
                table: "MemberQualificationType",
                column: "QualificationTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CustomerId",
                table: "Members",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_FirmId",
                table: "Members",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CommitteeID",
                table: "Messages",
                column: "CommitteeID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedById",
                table: "Messages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ThreadId",
                table: "Messages",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_CreatedById",
                table: "Penalties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_InvoiceRequestId",
                table: "Penalties",
                column: "InvoiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_MemberId",
                table: "Penalties",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_PenaltyTypeId",
                table: "Penalties",
                column: "PenaltyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_YearOfOperationId",
                table: "Penalties",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPayments_PenaltyId",
                table: "PenaltyPayments",
                column: "PenaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPayments_QBInvoiceId",
                table: "PenaltyPayments",
                column: "QBInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_CreatedById",
                table: "ProBonoApplications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_ProbonoClientId",
                table: "ProBonoApplications",
                column: "ProbonoClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_YearOfOperationId",
                table: "ProBonoApplications",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProbonoClients_CreatedById",
                table: "ProbonoClients",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_ApprovedById",
                table: "ProBonoReports",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_CreatedById",
                table: "ProBonoReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_ProBonoId",
                table: "ProBonoReports",
                column: "ProBonoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonos_CreatedById",
                table: "ProBonos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonos_ProBonoApplicationId",
                table: "ProBonos",
                column: "ProBonoApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonos_ProbonoClientId",
                table: "ProBonos",
                column: "ProbonoClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonos_YearOfOperationId",
                table: "ProBonos",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PropBonoReportFeedbacks_FeedBackById",
                table: "PropBonoReportFeedbacks",
                column: "FeedBackById");

            migrationBuilder.CreateIndex(
                name: "IX_PropBonoReportFeedbacks_ProBonoReportId",
                table: "PropBonoReportFeedbacks",
                column: "ProBonoReportId");

            migrationBuilder.CreateIndex(
                name: "IX_QBInvoices_CustomerId",
                table: "QBInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QBPayments_CustomerId",
                table: "QBPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QBPayments_InvoiceId",
                table: "QBPayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QBReceipts_CustomerId",
                table: "QBReceipts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QBReceipts_InvoiceId",
                table: "QBReceipts",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stamps_YearOfOperationId",
                table: "Stamps",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMemberships_MemberShipId",
                table: "SubcommitteeMemberships",
                column: "MemberShipId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMemberships_SubcommitteeID",
                table: "SubcommitteeMemberships",
                column: "SubcommitteeID");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMessages_CreatedById",
                table: "SubcommitteeMessages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMessages_SubcommitteeID",
                table: "SubcommitteeMessages",
                column: "SubcommitteeID");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMessages_SubcommitteeThreadId",
                table: "SubcommitteeMessages",
                column: "SubcommitteeThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcommittees_ChairpersonId",
                table: "Subcommittees",
                column: "ChairpersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcommittees_CommitteeId",
                table: "Subcommittees",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcommittees_CreatedById",
                table: "Subcommittees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeThreads_CreatedById",
                table: "SubcommitteeThreads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeThreads_SubcommitteeId",
                table: "SubcommitteeThreads",
                column: "SubcommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CommitteeId",
                table: "Threads",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CreatedById",
                table: "Threads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirmId",
                table: "Users",
                column: "FirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityTypeId",
                table: "Users",
                column: "IdentityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TitleId",
                table: "Users",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCPDTraining_Attachments_AttachmentsId",
                table: "AttachmentCPDTraining",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCPDTraining_CPDTrainings_CPDTrainingsId",
                table: "AttachmentCPDTraining",
                column: "CPDTrainingsId",
                principalTable: "CPDTrainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCPDTrainingRegistration_Attachments_AttachmentsId",
                table: "AttachmentCPDTrainingRegistration",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCPDTrainingRegistration_CPDTrainingRegistrations_C~",
                table: "AttachmentCPDTrainingRegistration",
                column: "CPDTrainingRegistrationsId",
                principalTable: "CPDTrainingRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentLevyDeclaration_Attachments_AttachmentsId",
                table: "AttachmentLevyDeclaration",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentLevyDeclaration_LevyDeclarations_LevyDeclarationsId",
                table: "AttachmentLevyDeclaration",
                column: "LevyDeclarationsId",
                principalTable: "LevyDeclarations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentLicenseApplication_Attachments_AttachmentsId",
                table: "AttachmentLicenseApplication",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentLicenseApplication_LicenseApplications_LicenseAppl~",
                table: "AttachmentLicenseApplication",
                column: "LicenseApplicationsId",
                principalTable: "LicenseApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentMemberQualification_Attachments_AttachmentsId",
                table: "AttachmentMemberQualification",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentMemberQualification_MemberQualifications_MemberQua~",
                table: "AttachmentMemberQualification",
                column: "MemberQualificationsId",
                principalTable: "MemberQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentMessage_Attachments_AttachmentsId",
                table: "AttachmentMessage",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentMessage_Messages_MessagesId",
                table: "AttachmentMessage",
                column: "MessagesId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentPenalty_Attachments_AttachmentsId",
                table: "AttachmentPenalty",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentPenalty_Penalties_PenaltiesId",
                table: "AttachmentPenalty",
                column: "PenaltiesId",
                principalTable: "Penalties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentPenaltyPayment_Attachments_AttachmentsId",
                table: "AttachmentPenaltyPayment",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentPenaltyPayment_PenaltyPayments_PenaltyPaymentsId",
                table: "AttachmentPenaltyPayment",
                column: "PenaltyPaymentsId",
                principalTable: "PenaltyPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBono_Attachments_AttachmentsId",
                table: "AttachmentProBono",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBono_ProBonos_ProBonosId",
                table: "AttachmentProBono",
                column: "ProBonosId",
                principalTable: "ProBonos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBonoApplication_Attachments_AttachmentsId",
                table: "AttachmentProBonoApplication",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBonoApplication_ProBonoApplications_ProBonosApp~",
                table: "AttachmentProBonoApplication",
                column: "ProBonosApplicationsId",
                principalTable: "ProBonoApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBonoReport_Attachments_AttachmentsId",
                table: "AttachmentProBonoReport",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentProBonoReport_ProBonoReports_ProBonoReportsId",
                table: "AttachmentProBonoReport",
                column: "ProBonoReportsId",
                principalTable: "ProBonoReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_SubcommitteeMessages_SubcommitteeMessageId",
                table: "Attachments",
                column: "SubcommitteeMessageId",
                principalTable: "SubcommitteeMessages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommitteeMembers_Committees_CommitteeID",
                table: "CommitteeMembers",
                column: "CommitteeID",
                principalTable: "Committees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommitteeMembers_Users_MemberShipId",
                table: "CommitteeMembers",
                column: "MemberShipId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Members_ChairpersonID",
                table: "Committees",
                column: "ChairpersonID",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Users_CreatedById",
                table: "Committees",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationMessages_Users_SentByUserId",
                table: "CommunicationMessages",
                column: "SentByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_CPDTrainings_CPDTrainingId",
                table: "CPDTrainingRegistrations",
                column: "CPDTrainingId",
                principalTable: "CPDTrainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_InvoiceRequests_InvoiceRequestId",
                table: "CPDTrainingRegistrations",
                column: "InvoiceRequestId",
                principalTable: "InvoiceRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_Members_MemberId",
                table: "CPDTrainingRegistrations",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_Users_CreatedById",
                table: "CPDTrainingRegistrations",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainings_Users_CreatedById",
                table: "CPDTrainings",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CPDUnitsEarned_Members_MemberId",
                table: "CPDUnitsEarned",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogs_Users_CreatedById",
                table: "ErrorLogs",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_Users_CreatedById",
                table: "Firms",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_Users_CreatedById",
                table: "Firms");

            migrationBuilder.DropTable(
                name: "AttachmentCPDTraining");

            migrationBuilder.DropTable(
                name: "AttachmentCPDTrainingRegistration");

            migrationBuilder.DropTable(
                name: "AttachmentLevyDeclaration");

            migrationBuilder.DropTable(
                name: "AttachmentLicenseApplication");

            migrationBuilder.DropTable(
                name: "AttachmentMemberQualification");

            migrationBuilder.DropTable(
                name: "AttachmentMessage");

            migrationBuilder.DropTable(
                name: "AttachmentPenalty");

            migrationBuilder.DropTable(
                name: "AttachmentPenaltyPayment");

            migrationBuilder.DropTable(
                name: "AttachmentProBono");

            migrationBuilder.DropTable(
                name: "AttachmentProBonoApplication");

            migrationBuilder.DropTable(
                name: "AttachmentProBonoReport");

            migrationBuilder.DropTable(
                name: "AttachmentStamp");

            migrationBuilder.DropTable(
                name: "CommitteeMembers");

            migrationBuilder.DropTable(
                name: "CommunicationMessages");

            migrationBuilder.DropTable(
                name: "CPDUnitsEarned");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "LevyPercents");

            migrationBuilder.DropTable(
                name: "LicenseApplicationApprovals");

            migrationBuilder.DropTable(
                name: "LicenseApprovalComments");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "MemberProBono");

            migrationBuilder.DropTable(
                name: "MemberQualificationType");

            migrationBuilder.DropTable(
                name: "PropBonoReportFeedbacks");

            migrationBuilder.DropTable(
                name: "QBPayments");

            migrationBuilder.DropTable(
                name: "QBReceipts");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SubcommitteeMemberships");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "CPDTrainingRegistrations");

            migrationBuilder.DropTable(
                name: "LevyDeclarations");

            migrationBuilder.DropTable(
                name: "MemberQualifications");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PenaltyPayments");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Stamps");

            migrationBuilder.DropTable(
                name: "LicenseApprovalHistories");

            migrationBuilder.DropTable(
                name: "ProBonoReports");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CPDTrainings");

            migrationBuilder.DropTable(
                name: "QualificationTypes");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "SubcommitteeMessages");

            migrationBuilder.DropTable(
                name: "LicenseApplications");

            migrationBuilder.DropTable(
                name: "ProBonos");

            migrationBuilder.DropTable(
                name: "InvoiceRequests");

            migrationBuilder.DropTable(
                name: "PenaltyTypes");

            migrationBuilder.DropTable(
                name: "SubcommitteeThreads");

            migrationBuilder.DropTable(
                name: "LicenseApprovalLevels");

            migrationBuilder.DropTable(
                name: "ProBonoApplications");

            migrationBuilder.DropTable(
                name: "QBInvoices");

            migrationBuilder.DropTable(
                name: "Subcommittees");

            migrationBuilder.DropTable(
                name: "ProbonoClients");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "YearOfOperations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Firms");

            migrationBuilder.DropTable(
                name: "IdentityTypes");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "QBCustomers");
        }
    }
}
