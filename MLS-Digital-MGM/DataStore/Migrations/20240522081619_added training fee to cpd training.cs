using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedtrainingfeetocpdtraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TrainingFee",
                table: "CPDTrainings",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "CPDTrainingRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CPDTrainingRegistrations_AttachmentId",
                table: "CPDTrainingRegistrations",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDTrainingRegistrations_Attachments_AttachmentId",
                table: "CPDTrainingRegistrations",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDTrainingRegistrations_Attachments_AttachmentId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_CPDTrainingRegistrations_AttachmentId",
                table: "CPDTrainingRegistrations");

            migrationBuilder.DropColumn(
                name: "TrainingFee",
                table: "CPDTrainings");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "CPDTrainingRegistrations");
        }
    }
}
