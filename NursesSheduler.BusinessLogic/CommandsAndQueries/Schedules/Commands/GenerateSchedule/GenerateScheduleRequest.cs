﻿using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.GenerateSchedule
{
    public sealed class GenerateScheduleRequest : IRequest<GenerateScheduleResponse>
    {
        public int ScheduleId { get; set; }
        public int DepartamentId { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; } 
        public int QuarterNumber { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public DayResponse[] MonthDays { get; set; }
        public MorningShiftsResponse[] MorningShifts { get; set; }
        public bool ReadOnly { get; set; }
        public ICollection<ScheduleNurseResponse> ScheduleNurses { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public string GeneratorSeed { get; set; }

        public sealed class DayResponse
        {
            public DateOnly Date { get; set; }
            public bool IsHoliday { get; set; }
            public string HolidayName { get; set; }
        }

        public sealed class ScheduleNurseResponse
        {
            public int ScheduleNurseId { get; set; }
            public int NurseId { get; init; }
            public NurseResponse Nurse { get; set; }
            public PreviousNurseStates PreviousState { get; set; }
            public int DaysFromLastShift { get; set; }

            public NurseWorkDayResponse[] NurseWorkDays { get; set; }
            public TimeSpan PreviousMonthTime { get; set; }
            public TimeSpan TimeToAssingInMonth { get; set; }
            public TimeSpan TimeToAssingInQuarterLeft { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
        }

        public sealed class NurseWorkDayResponse
        {
            public int NurseWorkDayId { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int DayNumber { get; set; }
            public TimeOnly ShiftStart { get; set; }
            public TimeOnly ShiftEnd { get; set; }
            public bool IsTimeOff { get; set; }
            public int MorningShiftId { get; set; }
        }

        public sealed class MorningShiftsResponse
        {
            public MorningShiftIndex Index { get; set; }
            public TimeSpan ShiftLength { get; set; }
        }

        public sealed class NurseResponse
        {
            public int NurseId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
