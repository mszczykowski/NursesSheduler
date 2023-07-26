using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class updatenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinmalShiftBreak",
                table: "DepartamentSettings",
                newName: "MinimalShiftBreak");

            migrationBuilder.RenameColumn(
                name: "MaximalWeekWorkTimeLength",
                table: "DepartamentSettings",
                newName: "MaximumWeekWorkTimeLength");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinimalShiftBreak",
                table: "DepartamentSettings",
                newName: "MinmalShiftBreak");

            migrationBuilder.RenameColumn(
                name: "MaximumWeekWorkTimeLength",
                table: "DepartamentSettings",
                newName: "MaximalWeekWorkTimeLength");
        }
    }
}
