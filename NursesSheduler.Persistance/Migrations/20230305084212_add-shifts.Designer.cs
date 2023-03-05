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
    [Migration("20230305084212_add-shifts")]
    partial class addshifts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("NurseShift", b =>
                {
                    b.Property<int>("AssignedNursesNurseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShiftsShiftId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssignedNursesNurseId", "ShiftsShiftId");

                    b.HasIndex("ShiftsShiftId");

                    b.ToTable("NurseShift");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Departament", b =>
                {
                    b.Property<int>("DepartamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("DepartamentId");

                    b.ToTable("Departaments");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Nurse", b =>
                {
                    b.Property<int>("NurseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DepartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("NurseId");

                    b.HasIndex("DepartamentId");

                    b.ToTable("Nurses");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Schedules.Schedule", b =>
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

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScheduleId");

                    b.HasIndex("DepartamentId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Schedules.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsShortShift")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("ShiftEnd")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("ShiftStart")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("ShiftId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.TimeOff", b =>
                {
                    b.Property<int>("TimeOffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("From")
                        .HasColumnType("TEXT");

                    b.Property<int>("NurseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("To")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("TimeOffId");

                    b.HasIndex("NurseId");

                    b.ToTable("TimeOff");
                });

            modelBuilder.Entity("NurseShift", b =>
                {
                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Nurse", null)
                        .WithMany()
                        .HasForeignKey("AssignedNursesNurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Schedules.Shift", null)
                        .WithMany()
                        .HasForeignKey("ShiftsShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Nurse", b =>
                {
                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Departament", "Departament")
                        .WithMany("Nurses")
                        .HasForeignKey("DepartamentId");

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Schedules.Schedule", b =>
                {
                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Departament", "Departament")
                        .WithMany("Schedules")
                        .HasForeignKey("DepartamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departament");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Schedules.Shift", b =>
                {
                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Schedules.Schedule", "Schedule")
                        .WithMany("Shifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.TimeOff", b =>
                {
                    b.HasOne("NursesScheduler.Domain.DatabaseModels.Nurse", "Nurse")
                        .WithMany("TimeOffs")
                        .HasForeignKey("NurseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nurse");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Departament", b =>
                {
                    b.Navigation("Nurses");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Nurse", b =>
                {
                    b.Navigation("TimeOffs");
                });

            modelBuilder.Entity("NursesScheduler.Domain.DatabaseModels.Schedules.Schedule", b =>
                {
                    b.Navigation("Shifts");
                });
#pragma warning restore 612, 618
        }
    }
}
