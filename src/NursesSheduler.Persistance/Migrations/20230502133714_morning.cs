using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    public partial class morning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MorningShifts_Quarters_QuarterId",
                table: "MorningShifts");

            migrationBuilder.AlterColumn<int>(
                name: "QuarterId",
                table: "MorningShifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "MorningShifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MorningShifts_Quarters_QuarterId",
                table: "MorningShifts",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "QuarterId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MorningShifts_Quarters_QuarterId",
                table: "MorningShifts");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "MorningShifts");

            migrationBuilder.AlterColumn<int>(
                name: "QuarterId",
                table: "MorningShifts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_MorningShifts_Quarters_QuarterId",
                table: "MorningShifts",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "QuarterId");
        }
    }
}
