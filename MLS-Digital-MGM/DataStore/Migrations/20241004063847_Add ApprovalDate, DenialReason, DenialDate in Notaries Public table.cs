using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalDateDenialReasonDenialDateinNotariesPublictable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationStatus",
                table: "NotariesPublic",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "NotariesPublic",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenialReason",
                table: "NotariesPublic",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeniedDate",
                table: "NotariesPublic",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "NotariesPublic");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "NotariesPublic");

            migrationBuilder.DropColumn(
                name: "DenialReason",
                table: "NotariesPublic");

            migrationBuilder.DropColumn(
                name: "DeniedDate",
                table: "NotariesPublic");
        }
    }
}
