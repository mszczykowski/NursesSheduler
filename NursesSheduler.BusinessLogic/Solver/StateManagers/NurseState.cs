using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Models.Settings;

namespace NursesScheduler.BusinessLogic.Solver.StateManagers
{
    internal sealed class NurseState : INurseState
    {
        public int NurseId { get; set; }

        public TimeSpan WorkTimeToAssign { get; set; }
        public TimeSpan WorkTimeToAssignInQuarter { get; set; }
        public TimeSpan PTOTimeToAssign { get; set; }
        public TimeSpan[] WorkTimeAssignedInWeek { get; set; }

        public TimeSpan HoursFromLastShift { get; set; }
        public TimeSpan HoursToNextShift { get; set; }

        public int NumberOfNightShifts { get; set; }
        public int NumberOfShiftsToAssign { get; set; }

        public TimeSpan HolidayPaidHoursAssigned { get; set; }

        public bool[] TimeOff { get; private set; }

        public List<TimeSpan> AssignedMorningShifts { get; set; }

        public NurseState(INurseState employee)
        {
            NurseId = employee.NurseId;
            WorkTimeToAssign = employee.WorkTimeToAssign;
            HoursFromLastShift = employee.HoursFromLastShift;
            HoursToNextShift = employee.HoursToNextShift;
            HolidayPaidHoursAssigned = employee.HolidayPaidHoursAssigned;
            NumberOfNightShifts = employee.NumberOfNightShifts;
            TimeOff = employee.TimeOff;
            AssignedMorningShifts = new List<TimeSpan>(employee.AssignedMorningShifts);
            NumberOfShiftsToAssign = employee.NumberOfShiftsToAssign;
            WorkTimeAssignedInWeek = new TimeSpan[employee.WorkTimeAssignedInWeek.Length];
            Array.Copy(employee.WorkTimeAssignedInWeek, WorkTimeAssignedInWeek, WorkTimeAssignedInWeek.Length);
        }

        public void UpdateStateOnAssign(TimeSpan shiftLenght, int weekInQuarter)
        {
            AssignedMorningShifts.Add(shiftLenght);
            UpdateWorkTimes(shiftLenght, weekInQuarter);
        }

        private void UpdateWorkTimes(TimeSpan shiftLenght, int weekInQuarter)
        {
            DaysFromLastShift = 0;

            WorkTimeToAssign -= shiftLenght;
            WorkTimeAssignedInWeek[weekInQuarter - 1] += shiftLenght;
        }

        public void AdvanceDaysFromLastShift()
        {
            DaysFromLastShift++;
        }

        public void UpdateStateOnAssign(bool isHoliday, ShiftIndex shiftIndex, 
            WorkTimeConfiguration workTimeConfiguration, int weekInQuarter)
        {
            if (isHoliday)
            {
                HolidayPaidHoursAssigned += workTimeConfiguration.ShiftDetails.Single(s => s.ShiftType == shiftType)
                    .HolidayEligibleHours;
            }

            if (shiftType == ShiftType.night) NumberOfNightShifts++;

            NumberOfShiftsToAssign--;

            UpdateWorkTimes(workTimeConfiguration.ShiftLenght, weekInQuarter);
        }
    }
}
