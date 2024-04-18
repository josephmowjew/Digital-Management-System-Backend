using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedyearofoperationtoprobono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "YearOfOperations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "YearOfOperations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "YearOfOperationId",
                table: "ProBonoApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearOfOperationId",
                table: "ProBono",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProBonoApplications_YearOfOperationId",
                table: "ProBonoApplications",
                column: "YearOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProBono_YearOfOperationId",
                table: "ProBono",
                column: "YearOfOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProBono_YearOfOperations_YearOfOperationId",
                table: "ProBono",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProBonoApplications_YearOfOperations_YearOfOperationId",
                table: "ProBonoApplications",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProBono_YearOfOperations_YearOfOperationId",
                table: "ProBono");

            migrationBuilder.DropForeignKey(
                name: "FK_ProBonoApplications_YearOfOperations_YearOfOperationId",
                table: "ProBonoApplications");

            migrationBuilder.DropIndex(
                name: "IX_ProBonoApplications_YearOfOperationId",
                table: "ProBonoApplications");

            migrationBuilder.DropIndex(
                name: "IX_ProBono_YearOfOperationId",
                table: "ProBono");

            migrationBuilder.DropColumn(
                name: "YearOfOperationId",
                table: "ProBonoApplications");

            migrationBuilder.DropColumn(
                name: "YearOfOperationId",
                table: "ProBono");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "YearOfOperations",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "YearOfOperations",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
