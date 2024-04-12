using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedprobonoclienttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProbonoClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PostalAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PermanentAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ResidentialAddress = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    OtherContacts = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    NatureOfDispute = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false),
                    CaseDetails = table.Column<string>(type: "longtext", nullable: false),
                    Occupation = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProbonoClients", x => x.Id);
                    
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProbonoClients_CreatedById",
                table: "ProbonoClients",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProbonoClients");
        }
    }
}
