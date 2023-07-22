using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.States
{
    internal sealed class NurseState : INurseState
    {
        public int NurseId { get; init; }
        public Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; init; }
        public TimeSpan HoursFromLastShift { get; init; }
        public TimeSpan[] HoursToNextShiftMatrix { get; init; }
        public int NumberOfRegularShiftsToAssign { get; init; }
        public int NumberOfTimeOffShiftsToAssign { get; init; }
        public TimeSpan HolidayHoursAssigned { get; init; }
        public TimeSpan NightHoursAssigned { get; init; }
        public TimeSpan WorkTimeInQuarterLeft { get; init; }
        public bool[] TimeOff { get; init; }
        public HashSet<MorningShiftIndex> PreviouslyAssignedMorningShifts { get; init; }
        public MorningShiftIndex? AssignedMorningShift { get; init; }
        public ShiftTypes PreviousMonthLastShift { get; init; }
        public NurseTeams NurseTeam { get; init; }

        public NurseState()
        {
            NurseId = scheduleNurse.NurseId;

            WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(nurseQuarterStats.WorkTimeAssignedInWeeks);

            HoursFromLastShift = previousScheduleNurseStats.HoursFromLastAssignedShift;

            HoursToNextShift = TimeSpan.Zero; // niew eim


        }

        public NurseState(INurseState nurse)
        {
            NurseId = nurse.NurseId;
            HoursFromLastShift = nurse.HoursFromLastShift;
            HoursToNextShift = nurse.HoursToNextShift;
            HolidayPaidHoursAssigned = nurse.HolidayPaidHoursAssigned;
            NumberOfNightShiftsAssigned = nurse.NumberOfNightShiftsAssigned;
            TimeOff = nurse.TimeOff;
            PreviouslyAssignedMorningShifts = new Dictionary<int, MorningShiftIndex>(nurse.AssignedMorningShifts);
            NumberOfRegularShiftsToAssign = nurse.NumberOfRegularShiftsToAssign;
            WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(nurse.WorkTimeAssignedInWeeks);
            HadMorningShiftAssigned = nurse.HadMorningShiftAssigned;
            NumberOfTimeOffShiftsToAssign = nurse.NumberOfTimeOffShiftsToAssign;
            PreviousMonthLastShift = nurse.PreviousMonthLastShift;
        }

        public void UpdateStateOnMorningShiftAssign(MorningShift morningShift, int weekInQuarter,
            TimeSpan hoursToNextShift)
        {
            HadMorningShiftAssigned = true;

            AssignedMorningShiftsIds.Add(morningShift.MorningShiftId);

            if (morningShift.ShiftLength > TimeSpan.Zero)
            {
                HoursFromLastShift = ScheduleConstatns.RegularShiftLenght - morningShift.ShiftLength;
                UpdateWorkTimes(morningShift.ShiftLength, weekInQuarter, hoursToNextShift);
            }
            else
            {
                AdvanceState(hoursToNextShift);
            }
        }

        public void UpdateStateOnTimeOffShiftAssign(bool isHoliday, CurrentShift shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift)
        {
            NumberOfTimeOffShiftsToAssign--;
            UpdateStateOnShiftAssign(isHoliday, shiftIndex, weekInQuarter, departamentSettings, hoursToNextShift);
        }

        public void UpdateStateOnRegularShiftAssign(bool isHoliday, CurrentShift shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift)
        {
            NumberOfRegularShiftsToAssign--;
            UpdateStateOnShiftAssign(isHoliday, shiftIndex, weekInQuarter,
                departamentSettings, hoursToNextShift, ScheduleConstatns.RegularShiftLenght);
        }

        public void UpdateStateOnShiftAssign(bool isWorkDay, CurrentShift currentShift, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan shiftLenght)
        {
            HoursFromLastShift = ScheduleConstatns.RegularShiftLenght - shiftLenght;

            if (currentShift == CurrentShift.Day)
            {
                if (isWorkDay)
                {

                }
            }

            switch (currentShift)
            {
                case CurrentShift.Day:
                    if (isWorkDay)
                    {
                        HolidayPaidHoursAssigned += departamentSettings.DayShiftHolidayEligibleHours;
                    }
                    break;
                case CurrentShift.Night:
                    if (isWorkDay)
                    {
                        HolidayPaidHoursAssigned += departamentSettings.NightShiftHolidayEligibleHours;
                    }
                    NumberOfNightShiftsAssigned++;
                    break;
            }

            UpdateWorkTimes(ScheduleConstatns.RegularShiftLenght, weekInQuarter, hoursToNextShift);
        }

        private void UpdateWorkTimes(TimeSpan shiftLenght, int weekInQuarter, TimeSpan hoursToNextShift)
        {
            WorkTimeAssignedInWeeks[weekInQuarter - 1] += shiftLenght;
            HoursToNextShift = hoursToNextShift;
        }

        public void AdvanceState(TimeSpan hoursToNextShift)
        {
            HoursFromLastShift += ScheduleConstatns.RegularShiftLenght;
            HoursToNextShift = hoursToNextShift;
        }

        public override bool Equals(object? obj)
        {
            return obj is NurseState state &&
                   NurseId == state.NurseId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NurseId);
        }

        public void ResetHoursFromLastShift()
        {
            HoursFromLastShift = TimeSpan.Zero;
        }
    }
}
