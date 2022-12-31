using SheduleSolver.Domain.Enums;
using SheduleSolver.Domain.Models;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.StateManagers
{
    public sealed class EmployeeState : IEmployeeState
    {
        public int Id { get; set; }

        public TimeSpan WorkTimeToAssign { get; set; }
        public TimeSpan WorkTimeToAssignInQuarter { get; set; }
        public TimeSpan PTOTimeToAssign { get; set; }
        public TimeSpan[] WorkTimeAssignedInWeek { get; set; }

        public int DaysFromLastShift { get; set; }
        public int NumberOfNightShifts { get; set; }
        public int NumberOfShiftsToAssign { get; set; }

        public TimeSpan HolidayPaidHoursAssigned { get; set; }

        public bool[] PTO { get; private set; }

        public List<TimeSpan> AssignedShortShifts { get; set; }

        public EmployeeState(int id)
        {
            Id = id;
            DaysFromLastShift = 0;
            NumberOfNightShifts = 0;
            HolidayPaidHoursAssigned = new TimeSpan(0, 0, 0);
            PTO = new bool[31];
            AssignedShortShifts = new List<TimeSpan>();
        }

        public EmployeeState(IEmployeeState employee)
        {
            Id = employee.Id;
            WorkTimeToAssign = employee.WorkTimeToAssign;
            DaysFromLastShift = employee.DaysFromLastShift;
            HolidayPaidHoursAssigned = employee.HolidayPaidHoursAssigned;
            NumberOfNightShifts = employee.NumberOfNightShifts;
            PTO = employee.PTO;
            AssignedShortShifts = new List<TimeSpan>(employee.AssignedShortShifts);
            NumberOfShiftsToAssign = employee.NumberOfShiftsToAssign;
            WorkTimeAssignedInWeek = new TimeSpan[employee.WorkTimeAssignedInWeek.Length];
            Array.Copy(employee.WorkTimeAssignedInWeek, WorkTimeAssignedInWeek, WorkTimeAssignedInWeek.Length);
        }

        public void UpdateStateOnAssign(bool isHoliday, ShiftType shiftType, WorkTimeConfiguration workTimeConfiguration,
            int weekInQuarter)
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

        public void UpdateStateOnAssign(TimeSpan shiftLenght, int weekInQuarter)
        {
            AssignedShortShifts.Add(shiftLenght);
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
    }
}
