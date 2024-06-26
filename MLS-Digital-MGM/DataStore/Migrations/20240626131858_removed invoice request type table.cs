using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class removedinvoicerequesttypetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_InvoiceRequests_InvoiceRequestTypes_InvoiceRequestTypeId",
            //     table: "InvoiceRequests");

            migrationBuilder.DropTable(
                name: "InvoiceRequestTypes");

            migrationBuilder.Sql("ALTER TABLE InvoiceRequests RENAME COLUMN InvoiceRequestTypeId TO YearOfOperationId;");
            // migrationBuilder.RenameColumn(
            //     name: "InvoiceRequestTypeId",
            //     table: "InvoiceRequests",
            //     newName: "YearOfOperationId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceRequests_InvoiceRequestTypeId",
                table: "InvoiceRequests",
                newName: "IX_InvoiceRequests_YearOfOperationId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InvoiceRequests",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "ReferencedEntityId",
                table: "InvoiceRequests",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ReferencedEntityType",
                table: "InvoiceRequests",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_YearOfOperations_YearOfOperationId",
                table: "InvoiceRequests",
                column: "YearOfOperationId",
                principalTable: "YearOfOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceRequests_YearOfOperations_YearOfOperationId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "ReferencedEntityId",
                table: "InvoiceRequests");

            migrationBuilder.DropColumn(
                name: "ReferencedEntityType",
                table: "InvoiceRequests");

            migrationBuilder.RenameColumn(
                name: "YearOfOperationId",
                table: "InvoiceRequests",
                newName: "InvoiceRequestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceRequests_YearOfOperationId",
                table: "InvoiceRequests",
                newName: "IX_InvoiceRequests_InvoiceRequestTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InvoiceRequests",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "InvoiceRequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRequestTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceRequests_InvoiceRequestTypes_InvoiceRequestTypeId",
                table: "InvoiceRequests",
                column: "InvoiceRequestTypeId",
                principalTable: "InvoiceRequestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
