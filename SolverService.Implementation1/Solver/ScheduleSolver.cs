using SheduleSolver.Domain.Enums;
using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;
using SheduleSolver.Implementation.Constraints;
using SheduleSolver.Implementation.Services;
using SheduleSolver.Implementation.StateManagers;
using SolverService.Interfaces.Constraints;
using SolverService.Interfaces.StateManagers;
using SolverService.Interfaces.Services;

namespace SolverService.Implementation.Solver
{
    internal sealed class ScheduleSolver
    {
        private List<IEmployeeState> _employees;
        private Quarter _quarter;
        private Month _month;
        private IWorkTimeService _workTimeService;
        private IEmployeeManagerService _employeeManagerService;
        private List<IConstraint> _constraints;
        private int currentEmployeeId;
        private IEmployeeState currentEmployee;
        private ShiftCapacities shiftCapacities;
        private readonly WorkTimeConfiguration _workTimeConfiguration;
        private Random random;

        public ScheduleSolver(List<IEmployeeState> employees, Quarter quarter)
        {
            _employees = employees;
            _quarter = quarter;
            _month = _quarter.Months[0];

            _employeeManagerService = new EmployeeManagerService();
            _workTimeConfiguration = new WorkTimeConfiguration();

            random = new Random();

            _workTimeService = new WorkTimeService(_quarter, 0, _workTimeConfiguration);

            _constraints = new List<IConstraint>
            {
                new HasShiftsToAssignLeft(),
                new BreakConstraint(),
                new MaxTotalHoursInWeekConstraint(),
            };

            _workTimeService.InitialiseShortShifts();
        }

        public ISolverState Run()
        {
            _workTimeService.InitialiseEmployeeTimeToAssign(_employees);

            _workTimeService.InitialiseShiftCapacities(_employees, random);

            var initialState = new SolverState(_employees, _quarter.Months[0].Days.Length,
                _workTimeConfiguration.ShiftDetails.Count);

            return AssignShift(initialState, new Queue<int>());
        }


        private ISolverState AssignShift(ISolverState previousState, Queue<int> previuousQueue)
        {
            if (previousState.CurrentDay > _month.Days.Length)
                return previousState;

            Queue<int> currentQueue;

            if (previousState.EmployeesToAssignForCurrentShift == 0 && previousState.ShortShiftsToAssign == 0)
            {
                previousState.EmployeesToAssignForCurrentShift = _workTimeService
                    .GetNumberOfNursesForShift((ShiftType)previousState.CurrentShift, previousState.CurrentDay);
                previousState.ShortShiftsToAssign = _workTimeService
                    .GetNumberOfShortShifts((ShiftType)previousState.CurrentShift, previousState.CurrentDay);
                currentQueue = _employeeManagerService
                    .GetSortedEmployeeQueue((ShiftType)previousState.CurrentShift,
                    _workTimeService.IsHoliday(_month.Days[previousState.CurrentDay - 1]),
                    previousState.GetPreviousDayShift(),
                    previousState.Employees.ToList(),
                    previousState.CurrentDay,
                    random);
            }
            else currentQueue = new Queue<int>(previuousQueue);

            var currentState = new SolverState(previousState);

            while (currentQueue.TryDequeue(out currentEmployeeId))
            {
                currentEmployee = currentState.Employees.Single(e => e.Id == currentEmployeeId);

                if (currentState.EmployeesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, currentEmployee, _month,
                        _workTimeConfiguration, _workTimeConfiguration.ShiftLenght)))
                    {
                        continue;
                    }

                    if (currentEmployee.NumberOfShiftsToAssign <= 0)
                        continue;

                    currentState.AssignEmployee(currentEmployee,
                        _workTimeService.IsHoliday(_month.Days[currentState.CurrentDay - 1]),
                        _workTimeConfiguration, _month.Days[currentState.CurrentDay - 1].WeekInQuarter);
                }
                else
                {
                    if (currentEmployee.AssignedShortShifts.Count == _month.MonthInQuarter)
                        continue;

                    var shortShiftsToAssign = _quarter.SurplusShifts.Except(currentEmployee.AssignedShortShifts).
                        OrderBy(s => random.Next()).ToList();

                    TimeSpan shiftLengthToAssingn = TimeSpan.Zero;

                    foreach (var shortShift in shortShiftsToAssign)
                    {
                        if (_constraints.All(c => c.IsSatisfied(currentState, currentEmployee, _month,
                            _workTimeConfiguration, shortShift)))
                        {
                            shiftLengthToAssingn = shortShift;
                            break;
                        }
                    }

                    if (shiftLengthToAssingn == TimeSpan.Zero) continue;

                    currentState.AssignEmployeeToShortShift(currentEmployee, shiftLengthToAssingn,
                        _month.Days[currentState.CurrentDay - 1].WeekInQuarter);
                }

                currentState.AdvanceState(_month);

                var result = AssignShift(currentState, currentQueue);

                if (result == null)
                {
                    currentState = new SolverState(previousState);
                    continue;
                }
                else return result;
            }

            return null;
        }

        private void AssignShiftsForPTO(ISolverState initialState, IEmployeeState employee)
        {
            int ptoStart;
            int ptoEnd;
            for (int i = 0; i < _month.Days.Length; i++)
            {
                if (employee.PTO[i] == true)
                {
                    ptoStart = i + 1;
                    while (employee.PTO[i] == true && employee.PTO.Length > i) i++;
                    ptoEnd = i + 1;
                }
            }
        }
    }
}
