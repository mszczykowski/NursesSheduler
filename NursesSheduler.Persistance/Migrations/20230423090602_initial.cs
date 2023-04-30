using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class initial : Migration
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
                name: "DepartamentSettings",
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
                    SettingsVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartamentSettings", x => x.DepartamentSettingsId);
                    table.ForeignKey(
                        name: "FK_DepartamentSettings_Departaments_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departaments",
                        principalColumn: "DepartamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PTOentitlement = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "Quarters",
                columns: table => new
                {
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuarterNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    QuarterYear = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SettingsVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    MorningShiftsReadOnly = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarters", x => x.QuarterId);
                    table.ForeignKey(
                        name: "FK_Quarters_Departaments_DepartamentId",
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
                name: "NursesQuartersStats",
                columns: table => new
                {
                    NurseQuarterStatsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkTimeInQuarterToAssign = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HolidayPaidHoursAssigned = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NumberOfNightShifts = table.Column<int>(type: "INTEGER", nullable: false),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursesQuartersStats", x => x.NurseQuarterStatsId);
                    table.ForeignKey(
                        name: "FK_NursesQuartersStats_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "NurseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NursesQuartersStats_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "QuarterId");
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
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkTimeInMonth = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TimeOffAvailableToAssgin = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TimeOffAssigned = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SettingsVersion = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Schedules_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "QuarterId",
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
                    AbsencesSummaryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.AbsenceId);
                    table.ForeignKey(
                        name: "FK_Absences_AbsencesSummaries_AbsencesSummaryId",
                        column: x => x.AbsencesSummaryId,
                        principalTable: "AbsencesSummaries",
                        principalColumn: "AbsencesSummaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MorningShifts",
                columns: table => new
                {
                    MorningShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Lenght = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NurseQuarterStatsId = table.Column<int>(type: "INTEGER", nullable: true),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MorningShifts", x => x.MorningShiftId);
                    table.ForeignKey(
                        name: "FK_MorningShifts_NursesQuartersStats_NurseQuarterStatsId",
                        column: x => x.NurseQuarterStatsId,
                        principalTable: "NursesQuartersStats",
                        principalColumn: "NurseQuarterStatsId");
                    table.ForeignKey(
                        name: "FK_MorningShifts_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "QuarterId");
                });

            migrationBuilder.CreateTable(
                name: "WorkTimeInWeek",
                columns: table => new
                {
                    WorkTimeInWeekId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeekNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedWorkTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NurseQuarterStatsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTimeInWeek", x => x.WorkTimeInWeekId);
                    table.ForeignKey(
                        name: "FK_WorkTimeInWeek_NursesQuartersStats_NurseQuarterStatsId",
                        column: x => x.NurseQuarterStatsId,
                        principalTable: "NursesQuartersStats",
                        principalColumn: "NurseQuarterStatsId");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleNurses",
                columns: table => new
                {
                    ScheduleNurseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleNurses", x => x.ScheduleNurseId);
                    table.ForeignKey(
                        name: "FK_ScheduleNurses_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "NurseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleNurses_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NursesWorkDays",
                columns: table => new
                {
                    NurseWorkDayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleNurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ShiftStart = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ShiftEnd = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MorningShiftId = table.Column<int>(type: "INTEGER", nullable: true),
                    AbsenceId = table.Column<int>(type: "INTEGER", nullable: true),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursesWorkDays", x => x.NurseWorkDayId);
                    table.ForeignKey(
                        name: "FK_NursesWorkDays_Absences_AbsenceId",
                        column: x => x.AbsenceId,
                        principalTable: "Absences",
                        principalColumn: "AbsenceId");
                    table.ForeignKey(
                        name: "FK_NursesWorkDays_MorningShifts_MorningShiftId",
                        column: x => x.MorningShiftId,
                        principalTable: "MorningShifts",
                        principalColumn: "MorningShiftId");
                    table.ForeignKey(
                        name: "FK_NursesWorkDays_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "NurseId");
                    table.ForeignKey(
                        name: "FK_NursesWorkDays_ScheduleNurses_ScheduleNurseId",
                        column: x => x.ScheduleNurseId,
                        principalTable: "ScheduleNurses",
                        principalColumn: "ScheduleNurseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_AbsencesSummaryId",
                table: "Absences",
                column: "AbsencesSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsencesSummaries_NurseId",
                table: "AbsencesSummaries",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartamentSettings_DepartamentId",
                table: "DepartamentSettings",
                column: "DepartamentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MorningShifts_NurseQuarterStatsId",
                table: "MorningShifts",
                column: "NurseQuarterStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_MorningShifts_QuarterId",
                table: "MorningShifts",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_DepartamentId",
                table: "Nurses",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesQuartersStats_NurseId",
                table: "NursesQuartersStats",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesQuartersStats_QuarterId",
                table: "NursesQuartersStats",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_AbsenceId",
                table: "NursesWorkDays",
                column: "AbsenceId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_MorningShiftId",
                table: "NursesWorkDays",
                column: "MorningShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_NurseId",
                table: "NursesWorkDays",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_ScheduleNurseId",
                table: "NursesWorkDays",
                column: "ScheduleNurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_DepartamentId",
                table: "Quarters",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleNurses_NurseId",
                table: "ScheduleNurses",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleNurses_ScheduleId",
                table: "ScheduleNurses",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DepartamentId",
                table: "Schedules",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_QuarterId",
                table: "Schedules",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimeInWeek_NurseQuarterStatsId",
                table: "WorkTimeInWeek",
                column: "NurseQuarterStatsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartamentSettings");

            migrationBuilder.DropTable(
                name: "NursesWorkDays");

            migrationBuilder.DropTable(
                name: "WorkTimeInWeek");

            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "MorningShifts");

            migrationBuilder.DropTable(
                name: "ScheduleNurses");

            migrationBuilder.DropTable(
                name: "AbsencesSummaries");

            migrationBuilder.DropTable(
                name: "NursesQuartersStats");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "Departaments");
        }
    }
}
