using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departaments",
                columns: table => new
                {
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departaments", x => x.DepartamentId);
                });

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    PTOentitlement = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.NurseId);
                    table.ForeignKey(
                        name: "FK_Nurses_Departaments_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departaments",
                        principalColumn: "DepartamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonthNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthInQuarter = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Holidays = table.Column<string>(type: "TEXT", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedules_Departaments_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departaments",
                        principalColumn: "DepartamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    DepartamentSettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MaximalWeekWorkingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MinmalShiftBreak = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    FirstQuarterStart = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstShiftStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TargetNumberOfNursesOnShift = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetMinimalMorningShiftLenght = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DefaultGeneratorRetryValue = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.DepartamentSettingsId);
                    table.ForeignKey(
                        name: "FK_Settings_Departaments_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departaments",
                        principalColumn: "DepartamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbsencesSummaries",
                columns: table => new
                {
                    AbsencesSummaryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    PTOTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PTOTimeUsed = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PTOTimeLeftFromPreviousYear = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsencesSummaries", x => x.AbsencesSummaryId);
                    table.ForeignKey(
                        name: "FK_AbsencesSummaries_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "NurseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsShortShift = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShiftStart = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ShiftEnd = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_Shifts_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    AbsenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    From = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    To = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    WorkingHoursToAssign = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AssignedWorkingHours = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    YearlyAbsencesSummaryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.AbsenceId);
                    table.ForeignKey(
                        name: "FK_Absences_AbsencesSummaries_YearlyAbsencesSummaryId",
                        column: x => x.YearlyAbsencesSummaryId,
                        principalTable: "AbsencesSummaries",
                        principalColumn: "AbsencesSummaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NurseShift",
                columns: table => new
                {
                    AssignedNursesNurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShiftsShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseShift", x => new { x.AssignedNursesNurseId, x.ShiftsShiftId });
                    table.ForeignKey(
                        name: "FK_NurseShift_Nurses_AssignedNursesNurseId",
                        column: x => x.AssignedNursesNurseId,
                        principalTable: "Nurses",
                        principalColumn: "NurseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseShift_Shifts_ShiftsShiftId",
                        column: x => x.ShiftsShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_YearlyAbsencesSummaryId",
                table: "Absences",
                column: "YearlyAbsencesSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsencesSummaries_NurseId",
                table: "AbsencesSummaries",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_DepartamentId",
                table: "Nurses",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseShift_ShiftsShiftId",
                table: "NurseShift",
                column: "ShiftsShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DepartamentId",
                table: "Schedules",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_DepartamentId",
                table: "Settings",
                column: "DepartamentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ScheduleId",
                table: "Shifts",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "NurseShift");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "AbsencesSummaries");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Departaments");
        }
    }
}
