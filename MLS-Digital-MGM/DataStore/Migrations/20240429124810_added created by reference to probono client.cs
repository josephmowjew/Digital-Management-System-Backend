using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatedbyreferencetoprobonoclient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "ProbonoClients",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProbonoClients",
                type: "varchar(200)",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProbonoClients_Users_CreatedById",
                table: "ProbonoClients");

            migrationBuilder.DropIndex(
                name: "IX_ProbonoClients_CreatedById",
                table: "ProbonoClients");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "ProbonoClients");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProbonoClients");
        }
    }
}
