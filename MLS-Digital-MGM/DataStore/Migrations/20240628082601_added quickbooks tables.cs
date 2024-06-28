using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedquickbookstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Members",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Firms",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QBCustomers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", nullable: false),
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
                name: "QBInvoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "longtext", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(250)", nullable: false),
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
                name: "QBPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(250)", nullable: false),
                    InvoiceId = table.Column<string>(type: "varchar(250)", nullable: false),
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
                    Id = table.Column<string>(type: "varchar(250)", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(250)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "longtext", nullable: false),
                    TotalPaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentId = table.Column<string>(type: "longtext", nullable: false),
                    InvoiceId = table.Column<string>(type: "varchar(250)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Members_CustomerId",
                table: "Members",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Firms_CustomerId",
                table: "Firms",
                column: "CustomerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_QBCustomers_CustomerId",
                table: "Firms",
                column: "CustomerId",
                principalTable: "QBCustomers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_QBCustomers_CustomerId",
                table: "Members",
                column: "CustomerId",
                principalTable: "QBCustomers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_QBCustomers_CustomerId",
                table: "Firms");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_QBCustomers_CustomerId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "QBPayments");

            migrationBuilder.DropTable(
                name: "QBReceipts");

            migrationBuilder.DropTable(
                name: "QBInvoices");

            migrationBuilder.DropTable(
                name: "QBCustomers");

            migrationBuilder.DropIndex(
                name: "IX_Members_CustomerId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Firms_CustomerId",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Firms");
        }
    }
}
