using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.StateManagers
{
    internal sealed class NurseState : INurseState
    {
        public int NurseId { get; init; }

        public Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; set; }

        public TimeSpan HoursFromLastShift { get; set; }
        public TimeSpan HoursToNextShift { get; set; }

        public int NumberOfNightShiftsAssigned { get; set; }
        public int NumberOfRegularShiftsToAssign { get; set; }
        public int NumberOfTimeOffShiftsToAssign { get; set; }

        public TimeSpan HolidayPaidHoursAssigned { get; set; }

        public bool[] TimeOff { get; private set; }

        public ICollection<int> AssignedMorningShiftsIds { get; set; }

        public bool HadMorningShiftAssigned { get; private set; }
        public ShiftTypes PreviousMonthLastShift { get; set; }


        public NurseState(ScheduleNurse scheduleNurse, NurseStats nurseQuarterStats, 
            NurseScheduleStats previousScheduleNurseStats)
        {
            NurseId = scheduleNurse.NurseId;

            WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(nurseQuarterStats.WorkTimeInWeeks);

            HoursFromLastShift = previousScheduleNurseStats.HoursFromLastShift;

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
            AssignedMorningShiftsIds = new List<int>(nurse.AssignedMorningShiftsIds);
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

            if(morningShift.ShiftLength > TimeSpan.Zero)
            {
                HoursFromLastShift = GeneralConstants.RegularShiftLenght - morningShift.ShiftLength;
                UpdateWorkTimes(morningShift.ShiftLength, weekInQuarter, hoursToNextShift);
            }
            else
            {
                AdvanceState(hoursToNextShift);
            }
        }

        public void UpdateStateOnTimeOffShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift)
        {
            NumberOfTimeOffShiftsToAssign--;
            UpdateStateOnRegularShiftAssign(isHoliday, shiftIndex, weekInQuarter, departamentSettings, hoursToNextShift);
        }

        public void UpdateStateOnRegularShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift)
        {
            HoursFromLastShift = TimeSpan.Zero;
            NumberOfRegularShiftsToAssign--;

            switch (shiftIndex)
            {
                case (ShiftIndex.Day):
                    if (isHoliday)
                    {
                        HolidayPaidHoursAssigned += departamentSettings.DayShiftHolidayEligibleHours;
                    }
                    break;
                case (ShiftIndex.Night):
                    if (isHoliday)
                    {
                        HolidayPaidHoursAssigned += departamentSettings.NightShiftHolidayEligibleHours;
                    }
                    NumberOfNightShiftsAssigned++;
                    break;
            }

            UpdateWorkTimes(GeneralConstants.RegularShiftLenght, weekInQuarter, hoursToNextShift);
        }

        private void UpdateWorkTimes(TimeSpan shiftLenght, int weekInQuarter, TimeSpan hoursToNextShift)
        {
            WorkTimeAssignedInWeeks[weekInQuarter - 1] += shiftLenght;
            HoursToNextShift = hoursToNextShift;
        }

        public void AdvanceState(TimeSpan hoursToNextShift)
        {
            HoursFromLastShift += GeneralConstants.RegularShiftLenght;
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
