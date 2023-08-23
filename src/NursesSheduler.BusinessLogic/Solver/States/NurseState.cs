﻿using NursesScheduler.BusinessLogic.Abstractions.Services;
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
        public Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; init; }

        public TimeSpan _hoursFromLastShift;
        public TimeSpan HoursFromLastShift { get; set; }
        public TimeSpan[] HoursToNextShiftMatrix { get; init; }
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

        public NurseState()
        {

        }

        public NurseState(INurseState stateToCopy)
        {
            //shallow copies
            NurseId = stateToCopy.NurseId;
            HoursToNextShiftMatrix = stateToCopy.HoursToNextShiftMatrix;
            HoursFromLastShift = stateToCopy.HoursFromLastShift;
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

            //deep copies
            WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(stateToCopy.WorkTimeAssignedInWeeks);
        }

        public void UpdateStateOnRegularShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfRegularShiftsToAssign--;
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLength, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnMorningShiftAssign(MorningShift morningShift, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService, bool shouldSwap)
        {
            if (AssignedMorningShiftId is not null)
            {
                throw new InvalidOperationException("UpdateStateOnMorningShiftAssign: assigned morning shift is not null");
            }

            if(shouldSwap && !HadNumberOfShiftsReduced)
            {
                NumberOfRegularShiftsToAssign--;
                HadNumberOfShiftsReduced = true;
            }

            AssignedMorningShiftId = morningShift.MorningShiftId;

            UpdateStateOnShiftAssign(ShiftTypes.Morning, morningShift.ShiftLength, day, departamentSettings,
                workTimeService);
        }

        public void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfTimeOffShiftsToAssign--;
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLength, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnShiftAssign(ShiftTypes shiftType, TimeSpan shiftLenght, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            WorkTimeAssignedInWeeks[day.WeekInQuarter] += shiftLenght;
            HoursFromLastShift = ScheduleConstatns.RegularShiftLength - shiftLenght;
            HolidayHoursAssigned += workTimeService.GetShiftHolidayHours(shiftType, shiftLenght, day, departamentSettings);
            NightHoursAssigned += workTimeService.GetShiftNightHours(shiftType, day, departamentSettings);
            WorkTimeInQuarterLeft -= shiftLenght;
        }

        public void AdvanceState()
        {
            HoursFromLastShift += ScheduleConstatns.RegularShiftLength;
        }

        private ShiftTypes GetShiftType(ShiftIndex shiftIndex)
        {
            if (shiftIndex == ShiftIndex.Day)
            {
                return ShiftTypes.Day;
            }
            else return ShiftTypes.Night;
        }

        public void ResetHoursFromLastShift()
        {
            HoursFromLastShift = TimeSpan.Zero;
        }
    }
}
