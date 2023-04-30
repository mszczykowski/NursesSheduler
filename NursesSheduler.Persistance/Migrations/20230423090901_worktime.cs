using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class worktime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimeInWeek_NursesQuartersStats_NurseQuarterStatsId",
                table: "WorkTimeInWeek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTimeInWeek",
                table: "WorkTimeInWeek");

            migrationBuilder.RenameTable(
                name: "WorkTimeInWeek",
                newName: "WorkTimeInWeeks");

            migrationBuilder.RenameIndex(
                name: "IX_WorkTimeInWeek_NurseQuarterStatsId",
                table: "WorkTimeInWeeks",
                newName: "IX_WorkTimeInWeeks_NurseQuarterStatsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTimeInWeeks",
                table: "WorkTimeInWeeks",
                column: "WorkTimeInWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimeInWeeks_NursesQuartersStats_NurseQuarterStatsId",
                table: "WorkTimeInWeeks",
                column: "NurseQuarterStatsId",
                principalTable: "NursesQuartersStats",
                principalColumn: "NurseQuarterStatsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimeInWeeks_NursesQuartersStats_NurseQuarterStatsId",
                table: "WorkTimeInWeeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTimeInWeeks",
                table: "WorkTimeInWeeks");

            migrationBuilder.RenameTable(
                name: "WorkTimeInWeeks",
                newName: "WorkTimeInWeek");

            migrationBuilder.RenameIndex(
                name: "IX_WorkTimeInWeeks_NurseQuarterStatsId",
                table: "WorkTimeInWeek",
                newName: "IX_WorkTimeInWeek_NurseQuarterStatsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTimeInWeek",
                table: "WorkTimeInWeek",
                column: "WorkTimeInWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimeInWeek_NursesQuartersStats_NurseQuarterStatsId",
                table: "WorkTimeInWeek",
                column: "NurseQuarterStatsId",
                principalTable: "NursesQuartersStats",
                principalColumn: "NurseQuarterStatsId");
        }
    }
}
