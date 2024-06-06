using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedupdatedfeeattributestocpdtraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE CPDTrainings RENAME COLUMN VirtualAttendanceFee TO NonMemberVirtualAttandanceFee;");
            // migrationBuilder.RenameColumn(
            //     name: "VirtualAttendanceFee",
            //     table: "CPDTrainings",
            //     newName: "NonMemberVirtualAttandanceFee");

            migrationBuilder.Sql("ALTER TABLE CPDTrainings RENAME COLUMN PhysicalAttendanceFee TO NonMemberPhysicalAttendanceFee;");
            // migrationBuilder.RenameColumn(
            //     name: "PhysicalAttendanceFee",
            //     table: "CPDTrainings",
            //     newName: "NonMemberPhysicalAttendanceFee");

            migrationBuilder.Sql("ALTER TABLE CPDTrainings RENAME COLUMN NonMemberFee TO MemberVirtualAttendanceFee;");
            // migrationBuilder.RenameColumn(
            //     name: "NonMemberFee",
            //     table: "CPDTrainings",
            //     newName: "MemberVirtualAttendanceFee");

            migrationBuilder.AddColumn<double>(
                name: "MemberPhysicalAttendanceFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberPhysicalAttendanceFee",
                table: "CPDTrainings");

            migrationBuilder.RenameColumn(
                name: "NonMemberVirtualAttandanceFee",
                table: "CPDTrainings",
                newName: "VirtualAttendanceFee");

            migrationBuilder.RenameColumn(
                name: "NonMemberPhysicalAttendanceFee",
                table: "CPDTrainings",
                newName: "PhysicalAttendanceFee");

            migrationBuilder.RenameColumn(
                name: "MemberVirtualAttendanceFee",
                table: "CPDTrainings",
                newName: "NonMemberFee");
        }
    }
}
