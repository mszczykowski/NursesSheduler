﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NursesScheduler.Infrastructure.Context;

#nullable disable

namespace NursesScheduler.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230813100823_summaryUpdate")]
    partial class summaryUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Days")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkTimeToAssign")
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

                    b.Property<TimeSpan>("PTOTimeLeft")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("PTOTimeLeftFromPreviousYear")
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

                    b.Property<int>("DefaultGeneratorTimeOut")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FirstQuarterStart")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("MaximumWeekWorkTimeLength")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("MinimalShiftBreak")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("NightShiftHolidayEligibleHours")
                        .HasColumnType("TEXT");

                    b.Property<int>("TargetMinNumberOfNursesOnShift")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TargetMinimalMorningShiftLenght")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseTeams")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("WorkDayLength")
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

                    b.Property<int>("QuarterId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ReadOnly")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("ShiftLength")
                        .HasColumnType("TEXT");

                    b.HasKey("MorningShiftId");

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

                    b.Property<TimeSpan>("HolidayHoursBalance")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("NightHoursBalance")
                        .HasColumnType("TEXT");

                    b.Property<int>("NurseTeam")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseWorkDay", b =>
                {
                    b.Property<int>("NurseWorkDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTimeOff")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MorningShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MorningShiftIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleNurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShiftType")
                        .HasColumnType("INTEGER");

                    b.HasKey("NurseWorkDayId");

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

                    b.Property<int>("QuarterNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("QuarterId");

                    b.HasIndex("DepartamentId");

                    b.ToTable("Quarters");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarterId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TimeOffAssigned")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("WorkTimeBalance")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("WorkTimeInMonth")
                        .HasColumnType("TEXT");

                    b.HasKey("ScheduleId");

                    b.HasIndex("QuarterId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.ScheduleNurse", b =>
                {
                    b.Property<int>("ScheduleNurseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("AssignedWorkTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("HolidaysHoursAssigned")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("NightHoursAssigned")
                        .HasColumnType("TEXT");

                    b.Property<int>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("PreviousMonthBalance")
                        .HasColumnType("TEXT");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TimeOffToAssiged")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TimeOffToAssign")
                        .HasColumnType("TEXT");

                    b.HasKey("ScheduleNurseId");

                    b.HasIndex("NurseId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("ScheduleNurses");
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

            modelBuilder.Entity("NursesScheduler.Domain.Entities.NurseWorkDay", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.MorningShift", "MorningShift")
                        .WithMany()
                        .HasForeignKey("MorningShiftId");

                    b.HasOne("NursesScheduler.Domain.Entities.Nurse", null)
                        .WithMany("NurseWorkDays")
                        .HasForeignKey("NurseId");

                    b.HasOne("NursesScheduler.Domain.Entities.ScheduleNurse", "ScheduleNurse")
                        .WithMany("NurseWorkDays")
                        .HasForeignKey("ScheduleNurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MorningShift");

                    b.Navigation("ScheduleNurse");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Quarter", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Departament", "Departament")
                        .WithMany("Quarters")
                        .HasForeignKey("DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("NursesScheduler.Domain.Entities.Quarter", "Quarter")
                        .WithMany("Schedules")
                        .HasForeignKey("QuarterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("NursesScheduler.Domain.Entities.AbsencesSummary", b =>
                {
                    b.Navigation("Absences");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Departament", b =>
                {
                    b.Navigation("DepartamentSettings")
                        .IsRequired();

                    b.Navigation("Nurses");

                    b.Navigation("Quarters");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Nurse", b =>
                {
                    b.Navigation("AbsencesSummaries");

                    b.Navigation("NurseWorkDays");
                });

            modelBuilder.Entity("NursesScheduler.Domain.Entities.Quarter", b =>
                {
                    b.Navigation("MorningShifts");

                    b.Navigation("Schedules");
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
