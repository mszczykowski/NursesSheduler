using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_YearlyAbsences_YearlyAbsencesId",
                table: "Absences");

            migrationBuilder.RenameColumn(
                name: "YearlyAbsencesId",
                table: "Absences",
                newName: "YearlyAbsencesSummaryId");

            migrationBuilder.RenameIndex(
                name: "IX_Absences_YearlyAbsencesId",
                table: "Absences",
                newName: "IX_Absences_YearlyAbsencesSummaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_YearlyAbsences_YearlyAbsencesSummaryId",
                table: "Absences",
                column: "YearlyAbsencesSummaryId",
                principalTable: "YearlyAbsences",
                principalColumn: "YearlyAbsencesSummaryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_YearlyAbsences_YearlyAbsencesSummaryId",
                table: "Absences");

            migrationBuilder.RenameColumn(
                name: "YearlyAbsencesSummaryId",
                table: "Absences",
                newName: "YearlyAbsencesId");

            migrationBuilder.RenameIndex(
                name: "IX_Absences_YearlyAbsencesSummaryId",
                table: "Absences",
                newName: "IX_Absences_YearlyAbsencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_YearlyAbsences_YearlyAbsencesId",
                table: "Absences",
                column: "YearlyAbsencesId",
                principalTable: "YearlyAbsences",
                principalColumn: "YearlyAbsencesSummaryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
