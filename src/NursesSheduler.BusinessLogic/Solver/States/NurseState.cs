using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Services;
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
        public TimeSpan HoursFromLastShift { get; private set; }
        public TimeSpan[] HoursToNextShiftMatrix { get; init; }
        public int NumberOfRegularShiftsToAssign { get; private set; }
        public int NumberOfTimeOffShiftsToAssign { get; private set; }
        public TimeSpan HolidayHoursAssigned { get; private set; }
        public TimeSpan NightHoursAssigned { get; private set; }
        public TimeSpan WorkTimeInQuarterLeft { get; private set; }
        public bool[] TimeOff { get; init; }
        public HashSet<MorningShiftIndex> PreviouslyAssignedMorningShifts { get; init; }
        public MorningShiftIndex? AssignedMorningShift { get; private set; }
        public ShiftTypes PreviousMonthLastShift { get; init; }
        public NurseTeams NurseTeam { get; init; }

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
            AssignedMorningShift = stateToCopy.AssignedMorningShift;
            PreviousMonthLastShift = stateToCopy.PreviousMonthLastShift;
            NurseTeam = stateToCopy.NurseTeam;

            //deep copies
            WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(stateToCopy.WorkTimeAssignedInWeeks);
        }

        public void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfRegularShiftsToAssign--;
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLenght, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnMorningShiftAssign(MorningShift morningShift, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            if (AssignedMorningShift is not null)
            {
                throw new InvalidOperationException("UpdateStateOnMorningShiftAssign: assigned morning shift is not null");
            }

            AssignedMorningShift = morningShift.Index;

            if(morningShift.ShiftLength == TimeSpan.Zero)
            {
                return;
            }

            UpdateStateOnShiftAssign(ShiftTypes.Morning, morningShift.ShiftLength, day, departamentSettings,
                workTimeService);
        }

        public void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            NumberOfTimeOffShiftsToAssign--;
            UpdateStateOnShiftAssign(GetShiftType(shiftIndex), ScheduleConstatns.RegularShiftLenght, day,
                departamentSettings, workTimeService);
        }

        public void UpdateStateOnShiftAssign(ShiftTypes shiftType, TimeSpan shiftLenght, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService)
        {
            WorkTimeAssignedInWeeks[day.WeekInQuarter] += shiftLenght;
            HoursFromLastShift = ScheduleConstatns.RegularShiftLenght - shiftLenght;
            HolidayHoursAssigned += workTimeService.GetShiftHolidayHours(shiftType, shiftLenght, day, departamentSettings);
            NightHoursAssigned += workTimeService.GetShiftNightHours(shiftType, day, departamentSettings);
            WorkTimeInQuarterLeft -= shiftLenght;
        }

        public void AdvanceState()
        {
            HoursFromLastShift += ScheduleConstatns.RegularShiftLenght;
        }

        private ShiftTypes GetShiftType(ShiftIndex shiftIndex)
        {
            if (shiftIndex == ShiftIndex.Day)
            {
                return ShiftTypes.Day;
            }
            else return ShiftTypes.Night;
        }
    }
}
