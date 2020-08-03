using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasContract.Editor.DataPersistence.DbContexts.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractFileSessions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SerializedContract = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractFileSessions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractFileSessions");
        }
    }
}
