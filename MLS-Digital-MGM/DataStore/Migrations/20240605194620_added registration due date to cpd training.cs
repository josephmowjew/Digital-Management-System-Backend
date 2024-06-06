using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedregistrationduedatetocpdtraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDueDate",
                table: "CPDTrainings",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDueDate",
                table: "CPDTrainings");
        }
    }
}
