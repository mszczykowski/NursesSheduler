using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class initialschema : Migration
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
                    WorkDayLength = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MaximumWeekWorkTimeLength = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MinimalShiftBreak = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TargetMinimalMorningShiftLenght = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DayShiftHolidayEligibleHours = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NightShiftHolidayEligibleHours = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TargetMinNumberOfNursesOnShift = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultGeneratorRetryValue = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultGeneratorTimeOut = table.Column<int>(type: "INTEGER", nullable: false),
                    UseTeams = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstQuarterStart = table.Column<int>(type: "INTEGER", nullable: false),
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
                    NurseTeam = table.Column<int>(type: "INTEGER", nullable: false),
                    NightHoursBalance = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HolidayHoursBalance = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    PTOTimeLeft = table.Column<TimeSpan>(type: "TEXT", nullable: false),
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
                name: "MorningShifts",
                columns: table => new
                {
                    MorningShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    ShiftLength = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ReadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MorningShifts", x => x.MorningShiftId);
                    table.ForeignKey(
                        name: "FK_MorningShifts_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "QuarterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkTimeInMonth = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WorkTimeBalance = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
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
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Days = table.Column<string>(type: "TEXT", nullable: false),
                    WorkTimeToAssign = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AssignedWorkingHours = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false),
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
                name: "ScheduleNurses",
                columns: table => new
                {
                    ScheduleNurseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedWorkTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TimeOffToAssign = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    TimeOffAssigned = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HolidayHoursAssigned = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    NightHoursAssigned = table.Column<TimeSpan>(type: "TEXT", nullable: false)
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
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTimeOff = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShiftType = table.Column<int>(type: "INTEGER", nullable: false),
                    MorningShiftIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    MorningShiftId = table.Column<int>(type: "INTEGER", nullable: true),
                    ScheduleNurseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursesWorkDays", x => x.NurseWorkDayId);
                    table.ForeignKey(
                        name: "FK_NursesWorkDays_MorningShifts_MorningShiftId",
                        column: x => x.MorningShiftId,
                        principalTable: "MorningShifts",
                        principalColumn: "MorningShiftId");
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
                name: "IX_MorningShifts_QuarterId",
                table: "MorningShifts",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_DepartamentId",
                table: "Nurses",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesWorkDays_MorningShiftId",
                table: "NursesWorkDays",
                column: "MorningShiftId");

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
                name: "IX_Schedules_QuarterId",
                table: "Schedules",
                column: "QuarterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "DepartamentSettings");

            migrationBuilder.DropTable(
                name: "NursesWorkDays");

            migrationBuilder.DropTable(
                name: "AbsencesSummaries");

            migrationBuilder.DropTable(
                name: "MorningShifts");

            migrationBuilder.DropTable(
                name: "ScheduleNurses");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "Departaments");
        }
    }
}
