using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedlicenceapplicationstatustrackingtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ChangedById = table.Column<string>(type: "varchar(200)", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseApprovalComments");

            migrationBuilder.DropTable(
                name: "LicenseApprovalHistories");
        }
    }
}
