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

        public TimeSpan _hoursFromLastShift;
        public TimeSpan HoursFromLastShift { get; set; }
        public TimeSpan[] HoursToNextShiftMatrix { get; set; }
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
            HolidayHoursAssigned += workTimeService.GetShiftHolidayHours(GetShiftType(shiftIndex), 
                ScheduleConstatns.RegularShiftLength, day, departamentSettings);
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLength, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnShiftAssign(ShiftTypes shiftType, TimeSpan shiftLenght, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            WorkTimeAssignedInWeeks[day.WeekInQuarter - 1] += shiftLenght;
            HoursFromLastShift = ScheduleConstatns.RegularShiftLength - shiftLenght;
            NightHoursAssigned += workTimeService.GetShiftNightHours(shiftType, day, departamentSettings);
            WorkTimeInQuarterLeft -= shiftLenght;

            ScheduleRow[day.Date.Day - 1] = shiftType;
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

        public void RecalculateHoursFromLastShift()
        {
            if(PreviousMonthLastShift == ShiftTypes.Night)
            {
                HoursFromLastShift = TimeSpan.Zero;
            }
            else
            {
                HoursFromLastShift = ScheduleConstatns.RegularShiftLength;
            }
        }

        public void ResetHoursToNextShiftMatrix(IWorkTimeService workTimeService)
        {
            bool shouldAddNextMonthHours;
            for (int i = 0; i < ScheduleRow.Count(); i++)
            {
                HoursToNextShiftMatrix[i] = workTimeService.GetHoursToFirstAssignedShift(i + 1, ScheduleRow);

                shouldAddNextMonthHours = true;
                for (int j = i; j < ScheduleRow.Count(); j++)
                {
                    if(ScheduleRow[j] != ShiftTypes.None)
                    {
                        shouldAddNextMonthHours = false;
                        break;
                    }
                }

                if(shouldAddNextMonthHours)
                {
                    HoursToNextShiftMatrix[i] += HoursToNextShiftMatrix.Last();
                }
            }
        }

        public void AssignMorningShift(int day, MorningShift morningShift)
        {
            ScheduleRow[day - 1] = ShiftTypes.Morning;
            AssignedMorningShiftId = morningShift.MorningShiftId;
        }
    }
}
