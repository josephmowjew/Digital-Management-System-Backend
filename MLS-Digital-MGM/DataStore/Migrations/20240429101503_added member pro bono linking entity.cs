using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedmemberprobonolinkingentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_MemberProBono_ProBonosId",
                table: "MemberProBono",
                column: "ProBonosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberProBono");
        }
    }
}
