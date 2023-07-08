using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.StateManagers
{
    internal sealed class NurseState : INurseState
    {
        public int NurseId { get; init; }

        public TimeSpan[] WorkTimeAssignedInWeeks { get; set; }

        public TimeSpan HoursFromLastShift { get; set; }
        public TimeSpan HoursToNextShift { get; set; }

        public int NumberOfNightShiftsAssigned { get; set; }
        public int NumberOfRegularShiftsToAssign { get; set; }
        public int NumberOfTimeOffShiftsToAssign { get; set; }

        public TimeSpan HolidayPaidHoursAssigned { get; set; }

        public bool[] TimeOff { get; private set; }

        public List<int> AssignedMorningShiftsIds { get; set; }

        public bool HadMorningShiftAssigned { get; private set; }

        public PreviousNurseStates PreviousMonthLastShift { get; set; }

        public NurseState(NurseQuarterStats nurseQuarterStats, ScheduleNurse previousScheduleNurse,
            ScheduleNurse currentScheduleNurse)
        {
            NurseId = nurseQuarterStats.NurseId;
            
            WorkTimeAssignedInWeeks = new TimeSpan[nurseQuarterStats.WorkTimeAssignedInWeeks.Count];
            var i = 0;
            foreach(var workTimeAssignedInWeek in nurseQuarterStats.WorkTimeAssignedInWeeks
                .OrderBy(t => t.WeekNumber)
                .Select(t => t.AssignedWorkTime))
            {
                WorkTimeAssignedInWeeks[i++] = workTimeAssignedInWeek;
            }

            if(previousScheduleNurse != null)
            {
                foreach (var workDay in previousScheduleNurse.NurseWorkDays.OrderByDescending(d => d.DayNumber))
                {
                    if (workDay.ShiftType == ShiftTypes.Night)
                    {
                        HoursFromLastShift += GeneralConstants.RegularShiftLenght;
                        break;
                    }
                    else if (workDay.ShiftType == ShiftTypes.Day)
                    {
                        break;
                    }
                    else if (workDay.ShiftType == ShiftTypes.Morning)
                    {
                        HoursFromLastShift += GeneralConstants.RegularShiftLenght - workDay.MorningShift.ShiftLength;
                        break;
                    }
                    HoursFromLastShift += GeneralConstants.RegularShiftLenght * 2;
                }
            }
            else
            {
                HoursFromLastShift = GeneralConstants.RegularShiftLenght * 10;
            }

            foreach (var workDay in currentScheduleNurse.NurseWorkDays.OrderBy(d => d.DayNumber))
            {
                if (workDay.ShiftType == ShiftTypes.Night)
                {
                    break;
                }
                if (workDay.ShiftType == ShiftTypes.Day || workDay.ShiftType == ShiftTypes.Morning)
                {
                    HoursFromLastShift += GeneralConstants.RegularShiftLenght;
                    break;
                }
                HoursFromLastShift += GeneralConstants.RegularShiftLenght * 2;
            }

            NumberOfNightShiftsAssigned = nurseQuarterStats.NumberOfNightShifts;

            NumberOfRegularShiftsToAssign = (int)Math.Floor(currentScheduleNurse.TimeToAssingInMonth / 
                GeneralConstants.RegularShiftLenght);
            NumberOfTimeOffShiftsToAssign = (int)Math.Round(currentScheduleNurse.TimeOffToAssign /
                GeneralConstants.RegularShiftLenght);

            HolidayPaidHoursAssigned = nurseQuarterStats.HolidayPaidHoursAssigned;

            TimeOff = new bool[currentScheduleNurse.NurseWorkDays.Count];
            foreach(var workDay in currentScheduleNurse.NurseWorkDays)
            {
                if(workDay.IsTimeOff)
                {
                    TimeOff[workDay.DayNumber - 1] = true;
                }
            }

            AssignedMorningShiftsIds = new(nurseQuarterStats.MorningShiftsAssigned.Select(m => m.Id));
            HadMorningShiftAssigned = currentScheduleNurse.NurseWorkDays.Any(d => d.ShiftType == ShiftTypes.Morning);

            PreviousMonthLastShift = currentScheduleNurse.PreviousState;
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
            WorkTimeAssignedInWeeks = new TimeSpan[nurse.WorkTimeAssignedInWeeks.Length];
            Array.Copy(nurse.WorkTimeAssignedInWeeks, WorkTimeAssignedInWeeks, WorkTimeAssignedInWeeks.Length);
            HadMorningShiftAssigned = nurse.HadMorningShiftAssigned;
            NumberOfTimeOffShiftsToAssign = nurse.NumberOfTimeOffShiftsToAssign;
            PreviousMonthLastShift = nurse.PreviousMonthLastShift;
        }

        public void UpdateStateOnMorningShiftAssign(MorningShift morningShift, int weekInQuarter,
            TimeSpan hoursToNextShift)
        {
            HadMorningShiftAssigned = true;

            AssignedMorningShiftsIds.Add(morningShift.Id);

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
