using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedextracolumnstocpdtraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE CPDTrainings RENAME COLUMN TrainingFee TO VirtualAttendanceFee;");
            //migrationBuilder.RenameColumn(
            //    name: "TrainingFee",
            //    table: "CPDTrainings",
            //    newName: "VirtualAttendanceFee");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "CPDTrainings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonMemberFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PhysicalAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "NonMemberFee",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "PhysicalAttendanceFee",
                table: "CPDTrainings");

            migrationBuilder.RenameColumn(
                name: "VirtualAttendanceFee",
                table: "CPDTrainings",
                newName: "TrainingFee");
        }
    }
}
