using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    DepartamentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nurses_Departaments_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departaments",
                        principalColumn: "Id");
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsShortShift = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShiftStart = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ShiftEnd = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_Shift_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeOff",
                columns: table => new
                {
                    TimeOffId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    NurseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeOff", x => x.TimeOffId);
                    table.ForeignKey(
                        name: "FK_TimeOff_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeOff_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "NurseShift",
                columns: table => new
                {
                    AssignedNursesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShiftsShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseShift", x => new { x.AssignedNursesId, x.ShiftsShiftId });
                    table.ForeignKey(
                        name: "FK_NurseShift_Nurses_AssignedNursesId",
                        column: x => x.AssignedNursesId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseShift_Shift_ShiftsShiftId",
                        column: x => x.ShiftsShiftId,
                        principalTable: "Shift",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Shift_ScheduleId",
                table: "Shift",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeOff_NurseId",
                table: "TimeOff",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeOff_ScheduleId",
                table: "TimeOff",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NurseShift");

            migrationBuilder.DropTable(
                name: "TimeOff");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Departaments");
        }
    }
}
