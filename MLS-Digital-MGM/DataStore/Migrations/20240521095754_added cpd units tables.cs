using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcpdunitstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ProposedUnits = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_CPDTrainings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPDTrainings_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CPDTrainingId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPDTrainingRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPDTrainingRegistrations_CPDTrainings_CPDTrainingId",
                        column: x => x.CPDTrainingId,
                        principalTable: "CPDTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPDTrainingRegistrations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CPDTrainingRegistrations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
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

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_CPDTrainingId",
                table: "CPDTrainingRegistrations",
                column: "CPDTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_CreatedById",
                table: "CPDTrainingRegistrations",
                column: "CreatedById");

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
                name: "IX_CPDUnitsEarned_YearOfOperationId",
                table: "CPDUnitsEarned",
                column: "YearOfOperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPDTrainingRegistrations");

            migrationBuilder.DropTable(
                name: "CPDUnitsEarned");

            migrationBuilder.DropTable(
                name: "CPDTrainings");
        }
    }
}
