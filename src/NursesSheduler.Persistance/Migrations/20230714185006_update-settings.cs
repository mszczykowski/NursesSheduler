using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class updatesettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstQuarterStart",
                table: "DepartamentSettings");

            migrationBuilder.AddColumn<int>(
                name: "FirstQuarterStart",
                table: "Departaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstQuarterStart",
                table: "Departaments");

            migrationBuilder.AddColumn<int>(
                name: "FirstQuarterStart",
                table: "DepartamentSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
