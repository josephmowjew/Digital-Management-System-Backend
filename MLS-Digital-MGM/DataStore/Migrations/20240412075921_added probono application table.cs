using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedprobonoapplicationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_ProbonoClients_CreatedById",
                table: "ProbonoClients");

            migrationBuilder.DropColumn(
                name: "CaseDetails",
                table: "ProbonoClients");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProbonoClients");

            migrationBuilder.DropColumn(
                name: "NatureOfDispute",
                table: "ProbonoClients");

            migrationBuilder.AddColumn<int>(
                name: "ProBonoApplicationId",
                table: "Attachments",
                type: "int",
                nullable: true);

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
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DenialReason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    SummaryOfDispute = table.Column<string>(type: "longtext", nullable: false),
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ProBonoApplicationId",
                table: "Attachments",
                column: "ProBonoApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_CreatedById",
                table: "ProBonoApplications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_ProbonoClientId",
                table: "ProBonoApplications",
                column: "ProbonoClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_ProBonoApplications_ProBonoApplicationId",
                table: "Attachments",
                column: "ProBonoApplicationId",
                principalTable: "ProBonoApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_ProBonoApplications_ProBonoApplicationId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "ProBonoApplications");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ProBonoApplicationId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ProBonoApplicationId",
                table: "Attachments");

            migrationBuilder.AddColumn<string>(
                name: "CaseDetails",
                table: "ProbonoClients",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProbonoClients",
                type: "varchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NatureOfDispute",
                table: "ProbonoClients",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProbonoClients_CreatedById",
                table: "ProbonoClients",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ProbonoClients_Users_CreatedById",
                table: "ProbonoClients",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
