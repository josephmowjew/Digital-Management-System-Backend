using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addsubcommitteeandsubcommitteemembershiptable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubcommitteeId",
                table: "Threads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubcommitteeId",
                table: "Messages",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Threads_SubcommitteeId",
                table: "Threads",
                column: "SubcommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SubcommitteeId",
                table: "Messages",
                column: "SubcommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMemberships_MemberShipId",
                table: "SubcommitteeMemberships",
                column: "MemberShipId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcommitteeMemberships_SubcommitteeID",
                table: "SubcommitteeMemberships",
                column: "SubcommitteeID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Subcommittees_SubcommitteeId",
                table: "Messages",
                column: "SubcommitteeId",
                principalTable: "Subcommittees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Subcommittees_SubcommitteeId",
                table: "Threads",
                column: "SubcommitteeId",
                principalTable: "Subcommittees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Subcommittees_SubcommitteeId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Subcommittees_SubcommitteeId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "SubcommitteeMemberships");

            migrationBuilder.DropTable(
                name: "Subcommittees");

            migrationBuilder.DropIndex(
                name: "IX_Threads_SubcommitteeId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SubcommitteeId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SubcommitteeId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "SubcommitteeId",
                table: "Messages");
        }
    }
}
