using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeniorandJuniorLawyerfeestotheCPDTrainingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCategorizedForMembers",
                table: "CPDTrainings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "JuniorLawyerPhysicalAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "JuniorLawyerVirtualAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SeniorLawyerPhysicalAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SeniorLawyerVirtualAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCategorizedForMembers",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "JuniorLawyerPhysicalAttendanceFee",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "JuniorLawyerVirtualAttendanceFee",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "SeniorLawyerPhysicalAttendanceFee",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "SeniorLawyerVirtualAttendanceFee",
                table: "CPDTrainings");
        }
    }
}
