using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddNotariesPublictable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotariesPublic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    YearOfOperationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotariesPublic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotariesPublic_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotariesPublic_YearOfOperations_YearOfOperationId",
                        column: x => x.YearOfOperationId,
                        principalTable: "YearOfOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttachmentNotaryPublic",
                columns: table => new
                {
                    AttachmentsId = table.Column<int>(type: "int", nullable: false),
                    NotariesPublicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentNotaryPublic", x => new { x.AttachmentsId, x.NotariesPublicId });
                    table.ForeignKey(
                        name: "FK_AttachmentNotaryPublic_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentNotaryPublic_NotariesPublic_NotariesPublicId",
                        column: x => x.NotariesPublicId,
                        principalTable: "NotariesPublic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentNotaryPublic_NotariesPublicId",
                table: "AttachmentNotaryPublic",
                column: "NotariesPublicId");

            migrationBuilder.CreateIndex(
                name: "IX_NotariesPublic_MemberId",
                table: "NotariesPublic",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NotariesPublic_YearOfOperationId",
                table: "NotariesPublic",
                column: "YearOfOperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentNotaryPublic");

            migrationBuilder.DropTable(
                name: "NotariesPublic");
        }
    }
}
