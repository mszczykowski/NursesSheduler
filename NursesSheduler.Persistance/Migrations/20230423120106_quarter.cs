using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class quarter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkTimeInQuarterToAssign",
                table: "Quarters",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkTimeInQuarterToAssign",
                table: "Quarters");
        }
    }
}
