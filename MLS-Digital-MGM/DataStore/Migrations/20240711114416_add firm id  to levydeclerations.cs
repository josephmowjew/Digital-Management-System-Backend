using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addfirmidtolevydeclerations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LevyDeclarations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "LevyDeclarations",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LevyDeclarations",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "LevyDeclarations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "firmId",
                table: "LevyDeclarations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LevyDeclarations_firmId",
                table: "LevyDeclarations",
                column: "firmId");

            migrationBuilder.AddForeignKey(
                name: "FK_LevyDeclarations_Firms_firmId",
                table: "LevyDeclarations",
                column: "firmId",
                principalTable: "Firms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LevyDeclarations_Firms_firmId",
                table: "LevyDeclarations");

            migrationBuilder.DropIndex(
                name: "IX_LevyDeclarations_firmId",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "LevyDeclarations");

            migrationBuilder.DropColumn(
                name: "firmId",
                table: "LevyDeclarations");
        }
    }
}
