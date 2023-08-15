using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class configuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NursesWorkDays_Nurses_NurseId",
                table: "NursesWorkDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleNurses_Nurses_NurseId",
                table: "ScheduleNurses");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleNurses_Schedules_ScheduleId",
                table: "ScheduleNurses");

            migrationBuilder.DropIndex(
                name: "IX_NursesWorkDays_NurseId",
                table: "NursesWorkDays");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "NursesWorkDays");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleNurses_Nurses_NurseId",
                table: "ScheduleNurses",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleNurses_Schedules_ScheduleId",
                table: "ScheduleNurses",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleNurses_Nurses_NurseId",
                table: "ScheduleNurses");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleNurses_Schedules_ScheduleId",
                table: "ScheduleNurses");

            migrationBuilder.AddColumn<int>(
                name: "NurseId",
                table: "NursesWorkDays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_NurseId",
                table: "NursesWorkDays",
                column: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_NursesWorkDays_Nurses_NurseId",
                table: "NursesWorkDays",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "NurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleNurses_Nurses_NurseId",
                table: "ScheduleNurses",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "NurseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleNurses_Schedules_ScheduleId",
                table: "ScheduleNurses",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
