using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.States
{
    internal sealed class NurseState : INurseState
    {
        public int NurseId { get; init; }
        public TimeSpan[] WorkTimeAssignedInWeeks { get; set; }
        public TimeSpan PreviousMonthHoursFromLastShift { get; set; }
        public TimeSpan NextMonthHoursToNextShift { get; set; }
        public TimeSpan HoursFromLastShift { get; private set; }
        public TimeSpan HoursToNextShift { get; private set; }
        public int NumberOfRegularShiftsToAssign { get; set; }
        public int NumberOfTimeOffShiftsToAssign { get; set; }
        public TimeSpan HolidayHoursAssigned { get; set; }
        public TimeSpan NightHoursAssigned { get; set; }
        public TimeSpan WorkTimeInQuarterLeft { get; set; }
        public bool[] TimeOff { get; init; }
        public HashSet<int> PreviouslyAssignedMorningShifts { get; init; }
        public int? AssignedMorningShiftId { get; set; }
        public ShiftTypes PreviousMonthLastShift { get; init; }
        public NurseTeams NurseTeam { get; init; }
        public bool HadNumberOfShiftsReduced { get; set; }
        public ShiftTypes[] ScheduleRow { get; set; }


        public bool ShouldNurseSwapRegularForMorning => !HadNumberOfShiftsReduced;

        public NurseState()
        {

        }

        public NurseState(INurseState stateToCopy)
        {
            //shallow copies
            NurseId = stateToCopy.NurseId;
            PreviousMonthHoursFromLastShift = stateToCopy.PreviousMonthHoursFromLastShift;
            NextMonthHoursToNextShift = stateToCopy.NextMonthHoursToNextShift;
            NumberOfRegularShiftsToAssign = stateToCopy.NumberOfRegularShiftsToAssign;
            NumberOfTimeOffShiftsToAssign = stateToCopy.NumberOfTimeOffShiftsToAssign;
            HolidayHoursAssigned = stateToCopy.HolidayHoursAssigned;
            NightHoursAssigned = stateToCopy.NightHoursAssigned;
            WorkTimeInQuarterLeft = stateToCopy.WorkTimeInQuarterLeft;
            TimeOff = stateToCopy.TimeOff;
            PreviouslyAssignedMorningShifts = stateToCopy.PreviouslyAssignedMorningShifts;
            AssignedMorningShiftId = stateToCopy.AssignedMorningShiftId;
            PreviousMonthLastShift = stateToCopy.PreviousMonthLastShift;
            NurseTeam = stateToCopy.NurseTeam;
            HadNumberOfShiftsReduced = stateToCopy.HadNumberOfShiftsReduced;
            HoursFromLastShift = stateToCopy.HoursFromLastShift;
            HoursToNextShift = stateToCopy.HoursToNextShift;

            //deep copies
            WorkTimeAssignedInWeeks = new TimeSpan[stateToCopy.WorkTimeAssignedInWeeks.Length];
            Array.Copy(stateToCopy.WorkTimeAssignedInWeeks, WorkTimeAssignedInWeeks, WorkTimeAssignedInWeeks.Length);

            ScheduleRow = new ShiftTypes[stateToCopy.ScheduleRow.Length];
            Array.Copy(stateToCopy.ScheduleRow, ScheduleRow, ScheduleRow.Length);
        }

        public void UpdateStateOnRegularShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfRegularShiftsToAssign--;
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLength, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnMorningShiftAssign(MorningShift morningShift, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            if (AssignedMorningShiftId is not null)
            {
                throw new InvalidOperationException("UpdateStateOnMorningShiftAssign: assigned morning shift is not null");
            }

            AssignedMorningShiftId = morningShift.MorningShiftId;

            UpdateStateOnShiftAssign(ShiftTypes.Morning, morningShift.ShiftLength, day, departamentSettings,
                workTimeService);
        }

        public void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfTimeOffShiftsToAssign--;
            HolidayHoursAssigned += workTimeService.GetShiftHolidayHours(GetShiftType(shiftIndex),
                ScheduleConstatns.RegularShiftLength, day, departamentSettings);
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLength, day,
                departamentSettings, workTimeService);
        }

        private void UpdateStateOnShiftAssign(ShiftTypes shiftType, TimeSpan shiftLenght, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            WorkTimeAssignedInWeeks[day.WeekInQuarter - 1] += shiftLenght;
            HoursFromLastShift = ScheduleConstatns.RegularShiftLength - shiftLenght;
            NightHoursAssigned += workTimeService.GetShiftNightHours(shiftType, day, departamentSettings);
            WorkTimeInQuarterLeft -= shiftLenght;

            ScheduleRow[day.Date.Day - 1] = shiftType;
        }

        public void RecalculateHoursFromLastShift(int day)
        {
            HoursFromLastShift = TimeSpan.Zero;

            for (int i = day - 2; i >= 0; i--)
            {
                switch (ScheduleRow[i])
                {
                    case ShiftTypes.None:
                        HoursFromLastShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        HoursFromLastShift += ScheduleConstatns.RegularShiftLength;
                        return;
                    case ShiftTypes.Morning:
                        HoursFromLastShift += ScheduleConstatns.RegularShiftLength;
                        return;
                    case ShiftTypes.Night:
                        return;
                }
            }

            HoursFromLastShift += PreviousMonthHoursFromLastShift;
        }

        public void RecalculateHoursToNextShift(int day)
        {
            HoursToNextShift = TimeSpan.Zero;

            for (int i = day - 1; i < ScheduleRow.Length; i++)
            {
                switch (ScheduleRow[i])
                {
                    case ShiftTypes.None:
                        HoursToNextShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        return;
                    case ShiftTypes.Morning:
                        return;
                    case ShiftTypes.Night:
                        HoursToNextShift += ScheduleConstatns.RegularShiftLength;
                        return;
                }
            }

            HoursToNextShift += NextMonthHoursToNextShift;
        }

        private ShiftTypes GetShiftType(ShiftIndex shiftIndex)
        {
            if (shiftIndex == ShiftIndex.Day)
            {
                return ShiftTypes.Day;
            }
            else return ShiftTypes.Night;
        }

        public void RecalculateFromPreviousAndToNextShift(int day)
        {
            RecalculateHoursFromLastShift(day);
            RecalculateHoursToNextShift(day);
        }
    }
}
