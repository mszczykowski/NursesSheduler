using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class summaryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PTOTime",
                table: "AbsencesSummaries");

            migrationBuilder.RenameColumn(
                name: "PTOTimeUsed",
                table: "AbsencesSummaries",
                newName: "PTOTimeLeft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PTOTimeLeft",
                table: "AbsencesSummaries",
                newName: "PTOTimeUsed");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PTOTime",
                table: "AbsencesSummaries",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
