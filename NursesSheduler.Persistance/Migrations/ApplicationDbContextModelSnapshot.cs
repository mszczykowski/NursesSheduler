﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NursesScheduler.Infrastructure.Context;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Absence", b =>
                {
                    b.Property<int>("AbsenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AbsencesSummaryId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("AssignedWorkingHours")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("From")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("To")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkingHoursToAssign")
                        .HasColumnType("TEXT");

                    b.HasKey("AbsenceId");

                    b.HasIndex("AbsencesSummaryId");

                    b.ToTable("Absences");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.AbsencesSummary", b =>
                {
                    b.Property<int>("AbsencesSummaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("PTOTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("PTOTimeLeftFromPreviousYear")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("PTOTimeUsed")
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("AbsencesSummaryId");

                    b.HasIndex("NurseId");

                    b.ToTable("AbsencesSummaries");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Departament", b =>
                {
                    b.Property<int>("DepartamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreationYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("DepartamentId");

                    b.ToTable("Departaments");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.DepartamentSettings", b =>
                {
                    b.Property<int>("DepartamentSettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("DayShiftHolidayEligibleHours")
                        .HasColumnType("TEXT");

                    b.Property<int>("DefaultGeneratorRetryValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FirstQuarterStart")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("MaximalWeekWorkingTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("MinmalShiftBreak")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("NightShiftHolidayEligibleHours")
                        .HasColumnType("TEXT");

                    b.Property<int>("SettingsVersion")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TargetMinimalMorningShiftLenght")
                        .HasColumnType("TEXT");

                    b.Property<int>("TargetNumberOfNursesOnShift")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkingTime")
                        .HasColumnType("TEXT");

                    b.HasKey("DepartamentSettingsId");

                    b.HasIndex("DepartamentId")
                        .IsUnique();

                    b.ToTable("DepartamentSettings");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.MorningShift", b =>
                {
                    b.Property<int>("MorningShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NurseQuarterStatsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarterId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("ShiftLength")
                        .HasColumnType("TEXT");

                    b.HasKey("MorningShiftId");

                    b.HasIndex("NurseQuarterStatsId");

                    b.HasIndex("QuarterId");

                    b.ToTable("MorningShifts");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Nurse", b =>
                {
                    b.Property<int>("NurseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("PTOentitlement")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("NurseId");

                    b.HasIndex("DepartamentId");

                    b.ToTable("Nurses");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseQuarterStats", b =>
                {
                    b.Property<int>("NurseQuarterStatsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("HolidayPaidHoursAssigned")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfNightShifts")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuarterId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkTimeInQuarterToAssign")
                        .HasColumnType("TEXT");

                    b.HasKey("NurseQuarterStatsId");

                    b.HasIndex("NurseId");

                    b.HasIndex("QuarterId");

                    b.ToTable("NursesQuartersStats");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseWorkDay", b =>
                {
                    b.Property<int>("NurseWorkDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AbsenceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MorningShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleNurseId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("ShiftEnd")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("ShiftStart")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("NurseWorkDayId");

                    b.HasIndex("AbsenceId");

                    b.HasIndex("MorningShiftId");

                    b.HasIndex("NurseId");

                    b.HasIndex("ScheduleNurseId");

                    b.ToTable("NursesWorkDays");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Quarter", b =>
                {
                    b.Property<int>("QuarterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MorningShiftsReadOnly")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarterNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarterYear")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SettingsVersion")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkTimeInQuarterToAssign")
                        .HasColumnType("TEXT");

                    b.HasKey("QuarterId");

                    b.HasIndex("DepartamentId");

                    b.ToTable("Quarters");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Holidays")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MonthInQuarter")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SettingsVersion")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TimeOffAssigned")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TimeOffAvailableToAssgin")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("WorkTimeInMonth")
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScheduleId");

                    b.HasIndex("DepartamentId");

                    b.HasIndex("QuarterId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.ScheduleNurse", b =>
                {
                    b.Property<int>("ScheduleNurseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScheduleNurseId");

                    b.HasIndex("NurseId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("ScheduleNurses");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.WorkTimeInWeek", b =>
                {
                    b.Property<int>("WorkTimeInWeekId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("AssignedWorkTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NurseQuarterStatsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("WorkTimeInWeekId");

                    b.HasIndex("NurseQuarterStatsId");

                    b.ToTable("WorkTimeInWeeks");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Absence", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.AbsencesSummary", "AbsencesSummary")
                        .WithMany("Absences")
                        .HasForeignKey("AbsencesSummaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AbsencesSummary");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.AbsencesSummary", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Nurse", "Nurse")
                        .WithMany("AbsencesSummaries")
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nurse");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.DepartamentSettings", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Departament", "Departament")
                        .WithOne("DepartamentSettings")
                        .HasForeignKey("NursesScheduler.Domain.Entities.DepartamentSettings", "DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.MorningShift", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.NurseQuarterStats", null)
                        .WithMany("MorningShiftsAssigned")
                        .HasForeignKey("NurseQuarterStatsId");

                    b.HasOne("NursesScheduler.Domain.Entities.Quarter", "Quarter")
                        .WithMany("MorningShifts")
                        .HasForeignKey("QuarterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quarter");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Nurse", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Departament", "Departament")
                        .WithMany("Nurses")
                        .HasForeignKey("DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseQuarterStats", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Nurse", "Nurse")
                        .WithMany()
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NursesScheduler.Domain.Entities.Quarter", null)
                        .WithMany("NurseQuarterStats")
                        .HasForeignKey("QuarterId");

                    b.Navigation("Nurse");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseWorkDay", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Absence", "Absence")
                        .WithMany("NurseWorkDays")
                        .HasForeignKey("AbsenceId");

                    b.HasOne("NursesScheduler.Domain.Entities.MorningShift", "MorningShift")
                        .WithMany()
                        .HasForeignKey("MorningShiftId");

                    b.HasOne("NursesScheduler.Domain.Entities.Nurse", null)
                        .WithMany("Shifts")
                        .HasForeignKey("NurseId");

                    b.HasOne("NursesScheduler.Domain.Entities.ScheduleNurse", "ScheduleNurse")
                        .WithMany("NurseWorkDays")
                        .HasForeignKey("ScheduleNurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Absence");

                    b.Navigation("MorningShift");

                    b.Navigation("ScheduleNurse");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Quarter", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Departament", "Departament")
                        .WithMany()
                        .HasForeignKey("DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Departament", "Departament")
                        .WithMany("Schedules")
                        .HasForeignKey("DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NursesScheduler.Domain.Entities.Quarter", "Quarter")
                        .WithMany()
                        .HasForeignKey("QuarterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");

                    b.Navigation("Quarter");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.ScheduleNurse", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Nurse", "Nurse")
                        .WithMany()
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NursesScheduler.Domain.Entities.Schedule", "Schedule")
                        .WithMany("ScheduleNurses")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nurse");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.WorkTimeInWeek", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.NurseQuarterStats", null)
                        .WithMany("WorkTimeAssignedInWeek")
                        .HasForeignKey("NurseQuarterStatsId");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Absence", b =>
                {
                    b.Navigation("NurseWorkDays");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.AbsencesSummary", b =>
                {
                    b.Navigation("Absences");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Departament", b =>
                {
                    b.Navigation("DepartamentSettings")
                        .IsRequired();

                    b.Navigation("Nurses");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Nurse", b =>
                {
                    b.Navigation("AbsencesSummaries");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseQuarterStats", b =>
                {
                    b.Navigation("MorningShiftsAssigned");

                    b.Navigation("WorkTimeAssignedInWeek");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Quarter", b =>
                {
                    b.Navigation("MorningShifts");

                    b.Navigation("NurseQuarterStats");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Schedule", b =>
                {
                    b.Navigation("ScheduleNurses");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.ScheduleNurse", b =>
                {
                    b.Navigation("NurseWorkDays");
                });
#pragma warning restore 612, 618
        }
    }
}
