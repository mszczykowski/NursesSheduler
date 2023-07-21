﻿using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface INurseState
    {
        int NurseId { get; }

        public Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; }

        TimeSpan HoursFromLastShift { get; set; }
        TimeSpan HoursToNextShift { get; set; }

        TimeSpan HolidayPaidHoursAssigned { get; set; }
        
        int NumberOfNightShiftsAssigned { get; set; }
        int NumberOfRegularShiftsToAssign { get; set; }
        int NumberOfTimeOffShiftsToAssign { get; set; }
        
        bool[] TimeOff { get; }
        bool HadMorningShiftAssigned { get; }
        ShiftTypes PreviousMonthLastShift { get; }

        NurseTeams NurseTeam { get; }

        IDictionary<int, MorningShiftIndex> AssignedMorningShifts { get; set; }

        TimeSpan WorkTimeInQuarterLeft { get; set; }

        void AdvanceState(TimeSpan hoursToNextShift);
        void UpdateStateOnMorningShiftAssign(MorningShift morningShift, int weekInQuarter, 
            TimeSpan hoursToNextShift);
        void UpdateStateOnRegularShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter, 
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void UpdateStateOnTimeOffShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter, 
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void ResetHoursFromLastShift();
    }
}