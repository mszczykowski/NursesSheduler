using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstShiftStartTime",
                table: "DepartamentSettings",
                newName: "NightShiftHolidayEligibleHours");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DayShiftHolidayEligibleHours",
                table: "DepartamentSettings",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayShiftHolidayEligibleHours",
                table: "DepartamentSettings");

            migrationBuilder.RenameColumn(
                name: "NightShiftHolidayEligibleHours",
                table: "DepartamentSettings",
                newName: "FirstShiftStartTime");
        }
    }
}
