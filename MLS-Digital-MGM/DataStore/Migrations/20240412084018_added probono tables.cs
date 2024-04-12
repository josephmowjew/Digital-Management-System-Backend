using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedprobonotables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ProBonoApplicationId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ProBonoApplicationId",
                table: "Attachments");

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
                    table.ForeignKey(
                        name: "FK_AttachmentProBonoApplication_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentProBonoApplication_ProBonoApplications_ProBonosApp~",
                        column: x => x.ProBonosApplicationsId,
                        principalTable: "ProBonoApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProBono",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NatureOfDispute = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CaseDetails = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: true),
                    ProbonoClientId = table.Column<int>(type: "int", nullable: false),
                    SummaryOfDispute = table.Column<string>(type: "longtext", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProBono", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProBono_ProbonoClients_ProbonoClientId",
                        column: x => x.ProbonoClientId,
                        principalTable: "ProbonoClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBono_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_AttachmentProBono_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentProBono_ProBono_ProBonosId",
                        column: x => x.ProBonosId,
                        principalTable: "ProBono",
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
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProBonoReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProBonoReports_ProBono_ProBonoId",
                        column: x => x.ProBonoId,
                        principalTable: "ProBono",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProBonoReports_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "IX_AttachmentProBono_ProBonosId",
                table: "AttachmentProBono",
                column: "ProBonosId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentProBonoApplication_ProBonosApplicationsId",
                table: "AttachmentProBonoApplication",
                column: "ProBonosApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBono_CreatedById",
                table: "ProBono",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBono_ProbonoClientId",
                table: "ProBono",
                column: "ProbonoClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_ApprovedById",
                table: "ProBonoReports",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoReports_ProBonoId",
                table: "ProBonoReports",
                column: "ProBonoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropBonoReportFeedbacks_FeedBackById",
                table: "PropBonoReportFeedbacks",
                column: "FeedBackById");

            migrationBuilder.CreateIndex(
                name: "IX_PropBonoReportFeedbacks_ProBonoReportId",
                table: "PropBonoReportFeedbacks",
                column: "ProBonoReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentProBono");

            migrationBuilder.DropTable(
                name: "AttachmentProBonoApplication");

            migrationBuilder.DropTable(
                name: "PropBonoReportFeedbacks");

            migrationBuilder.DropTable(
                name: "ProBonoReports");

            migrationBuilder.DropTable(
                name: "ProBono");

            migrationBuilder.AddColumn<int>(
                name: "ProBonoApplicationId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ProBonoApplicationId",
                table: "Attachments",
                column: "ProBonoApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_ProBonoApplications_ProBonoApplicationId",
                table: "Attachments",
                column: "ProBonoApplicationId",
                principalTable: "ProBonoApplications",
                principalColumn: "Id");
        }
    }
}
