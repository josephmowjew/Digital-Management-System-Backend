using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcustomerreferencetofirms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<string>(
            //     name: "Description",
            //     table: "InvoiceRequests",
            //     type: "varchar(250)",
            //     maxLength: 250,
            //     nullable: true,
            //     oldClrType: typeof(string),
            //     oldType: "varchar(250)",
            //     oldMaxLength: 250);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<string>(
            //     name: "Description",
            //     table: "InvoiceRequests",
            //     type: "varchar(250)",
            //     maxLength: 250,
            //     nullable: false,
            //     defaultValue: "",
            //     oldClrType: typeof(string),
            //     oldType: "varchar(250)",
            //     oldMaxLength: 250,
            //     oldNullable: true);
        }
    }
}
