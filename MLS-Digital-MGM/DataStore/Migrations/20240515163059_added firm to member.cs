﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedfirmtomember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_FirmId",
                table: "Members",
                column: "FirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Firms_FirmId",
                table: "Members",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Firms_FirmId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_FirmId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Members");
        }
    }
}
