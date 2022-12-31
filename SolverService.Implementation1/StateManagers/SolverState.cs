using SheduleSolver.Domain.Enums;
using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.StateManagers
{
    public sealed class SolverState : ISolverState
    {
        public int CurrentDay { get; set; }
        public int CurrentShift { get; set; }
        public int EmployeesToAssignForCurrentShift { get; set; }
        public int ShortShiftsToAssign { get; set; }
        public List<IEmployeeState> Employees { get; }
        public List<int>[,] ScheduleState { get; }
        public List<int>[] AssignedShortShifts { get; }
        public SolverState(List<IEmployeeState> employees, int numberOfDays, int numberOfShifts)
        {
            CurrentDay = 1;
            CurrentShift = 0;
            EmployeesToAssignForCurrentShift = 0;
            Employees = new List<IEmployeeState>();
            employees.ForEach(e =>
            {
                Employees.Add(new EmployeeState(e));
            });
            ScheduleState = new List<int>[numberOfDays, numberOfShifts];
            AssignedShortShifts = new List<int>[numberOfDays];
        }

        public SolverState(ISolverState stateToCopy)
        {
            CurrentDay = stateToCopy.CurrentDay;

            CurrentShift = stateToCopy.CurrentShift;

            EmployeesToAssignForCurrentShift = stateToCopy.EmployeesToAssignForCurrentShift;

            Employees = new List<IEmployeeState>();

            foreach (var e in stateToCopy.Employees)
            {
                Employees.Add(new EmployeeState(e));
            }

            ScheduleState = new List<int>[stateToCopy.ScheduleState.GetLength(0),
                stateToCopy.ScheduleState.GetLength(1)];

            for (int i = 0; i < ScheduleState.GetLength(0); i++)
            {
                for (int j = 0; j < ScheduleState.GetLength(1); j++)
                {
                    if (stateToCopy.ScheduleState[i, j] != null)
                    {
                        ScheduleState[i, j] = new List<int>(stateToCopy.ScheduleState[i, j]);
                    }
                }
            }

            AssignedShortShifts = new List<int>[stateToCopy.AssignedShortShifts.Length];

            for (int i = 0; i < AssignedShortShifts.Length; i++)
            {
                if (stateToCopy.AssignedShortShifts[i] != null)
                {
                    AssignedShortShifts[i] = new List<int>(stateToCopy.AssignedShortShifts[i]);
                }
            }

            ShortShiftsToAssign = stateToCopy.ShortShiftsToAssign;
        }

        public void AdvanceState(Month currentMonth)
        {
            if (EmployeesToAssignForCurrentShift > 0)
            {
                EmployeesToAssignForCurrentShift--;
            }
            else
            {
                ShortShiftsToAssign--;
            }
            if (EmployeesToAssignForCurrentShift <= 0 && ShortShiftsToAssign <= 0)
            {
                CurrentShift++;
                if (CurrentShift >= ScheduleState.GetLength(1))
                {
                    CurrentShift = 0;
                    CurrentDay++;

                    if (CurrentDay - 1 < ScheduleState.GetLength(0))
                    {
                        foreach (var e in Employees)
                        {
                            e.AdvanceDaysFromLastShift();
                        }
                    }
                }
            }
        }

        public List<int> GetPreviousDayShift()
        {
            if (CurrentDay == 1) return new List<int>();

            List<int> result;

            if (CurrentShift == (int)ShiftType.day)
            {
                result = ScheduleState[CurrentDay - 2, (int)ShiftType.day];
                if (AssignedShortShifts[CurrentDay - 2] != null) result.AddRange(AssignedShortShifts[CurrentDay - 2]);
            }
            else
            {
                result = ScheduleState[CurrentDay - 1, (int)ShiftType.day];
                if (AssignedShortShifts[CurrentDay - 1] != null) result.AddRange(AssignedShortShifts[CurrentDay - 1]);
            }

            return result;
        }

        public void AssignEmployee(IEmployeeState employee, bool isHoliday, WorkTimeConfiguration workTimeConfiguration,
            int weekInQuarter)
        {
            if (ScheduleState[CurrentDay - 1, CurrentShift] == null)
                ScheduleState[CurrentDay - 1, CurrentShift] = new List<int> { employee.Id };

            else ScheduleState[CurrentDay - 1, CurrentShift].Add(employee.Id);

            employee.UpdateStateOnAssign(isHoliday, (ShiftType)CurrentShift, workTimeConfiguration, weekInQuarter);
        }

        public void AssignEmployeeToShortShift(IEmployeeState employee, TimeSpan shortShiftLenght,
            int weekInQuarter)
        {
            if (AssignedShortShifts[CurrentDay - 1] == null)
                AssignedShortShifts[CurrentDay - 1] = new List<int> { employee.Id };

            else AssignedShortShifts[CurrentDay - 1].Add(employee.Id);

            employee.UpdateStateOnAssign(shortShiftLenght, weekInQuarter);
        }

        public List<int> GetPreviousShift()
        {
            int previousShift = CurrentShift - 1 < 0 ? 1 : 0;
            int previousDay = previousShift == 0 ? CurrentDay : CurrentDay - 1;
            if (previousDay - 1 < 0) return null;
            return ScheduleState[previousDay - 1, previousShift];
        }
        public List<int> GetNextShift()
        {
            int nextShift = CurrentShift + 1 > 1 ? 0 : 1;
            int nextDay = nextShift == 1 ? CurrentDay : CurrentDay + 1;
            if (nextDay - 1 >= ScheduleState.GetLength(0)) return null;
            return ScheduleState[nextDay - 1, nextShift];
        }
    }
}
