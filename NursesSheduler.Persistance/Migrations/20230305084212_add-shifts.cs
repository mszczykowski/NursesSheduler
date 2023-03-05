using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class addshifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NurseShift_Shift_ShiftsShiftId",
                table: "NurseShift");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Schedules_ScheduleId",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_ScheduleId",
                table: "Shifts",
                newName: "IX_Shifts_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseShift_Shifts_ShiftsShiftId",
                table: "NurseShift",
                column: "ShiftsShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Schedules_ScheduleId",
                table: "Shifts",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NurseShift_Shifts_ShiftsShiftId",
                table: "NurseShift");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Schedules_ScheduleId",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_ScheduleId",
                table: "Shift",
                newName: "IX_Shift_ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_NurseShift_Shift_ShiftsShiftId",
                table: "NurseShift",
                column: "ShiftsShiftId",
                principalTable: "Shift",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Schedules_ScheduleId",
                table: "Shift",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
