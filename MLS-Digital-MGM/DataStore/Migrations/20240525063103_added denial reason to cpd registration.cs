using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addeddenialreasontocpdregistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeniedReason",
                table: "CPDTrainingRegistrations",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeniedReason",
                table: "CPDTrainingRegistrations");
        }
    }
}
