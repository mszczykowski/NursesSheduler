using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.BusinessLogic.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Solver
{
    internal sealed class ScheduleSolver
    {
        private List<INurseState> _nurses;
        private List<IConstraint> _constraints;
        private INurseQueueDirector _employeeQueueDirector;
        private Random _random;
        private DepartamentSettings _departamentSettings;

        private Quarter _quarter;
        private Day[] _month;

        private MorningShift[] _morningShifts;

        private int currentEmployeeId;
        private INurseState currentEmployee;

        private readonly IShiftCapacityManager _shiftCapacityManager;

        public ScheduleSolver(List<INurseState> nurses, Quarter quarter, Day[] month, List<IConstraint> constraints,
            DepartamentSettings departamentSettings, MorningShift[] morningShifts, Random random)
        {
            _nurses = nurses;
            _quarter = quarter;
            _month = month;
            _departamentSettings = departamentSettings;
            _constraints = constraints;

            _morningShifts = morningShifts;

            _random = random;

            _shiftCapacityManager = new ShiftCapacityManager(_departamentSettings, _month, _nurses, _random);
            _employeeQueueDirector = new NurseQueueDirector(_random);
        }

        public ISolverState Run()
        {
            _shiftCapacityManager.InitialiseShiftCapacities();

            var initialState = new SolverState(_nurses, _month.Length, GeneralConstants.NumberOfShifts);

            return AssignShift(initialState, new Queue<int>());
        }


        private ISolverState AssignShift(ISolverState previousState, Queue<int> previuousQueue)
        {
            if (previousState.CurrentDay > _month.Length)
                return previousState;

            Queue<int> currentQueue;

            if (previousState.EmployeesToAssignForCurrentShift == 0 && previousState.ShortShiftsToAssign == 0)
            {
                previousState.EmployeesToAssignForCurrentShift = _shiftCapacityManager
                    .GetNumberOfNursesForShift((ShiftIndex)previousState.CurrentShift, previousState.CurrentDay);

                previousState.ShortShiftsToAssign = _shiftCapacityManager
                    .GetNumberOfNursesForShift(ShiftIndex.Morning, previousState.CurrentDay);
                
                currentQueue = _employeeQueueDirector
                    .GetSortedEmployeeQueue((ShiftIndex)previousState.CurrentShift,
                    !_month[previousState.CurrentDay - 1].IsWorkingDay,
                    previousState.GetPreviousDayShift(),
                    previousState.Nurses.ToList(),
                    previousState.CurrentDay);
            }
            else currentQueue = new Queue<int>(previuousQueue);

            var currentState = new SolverState(previousState);

            while (currentQueue.TryDequeue(out currentEmployeeId))
            {
                currentEmployee = currentState.Nurses.Single(e => e.NurseId == currentEmployeeId);

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
                        _workTimeHelper.IsHoliday(_month.Days[currentState.CurrentDay - 1]),
                        _workTimeConfiguration, _month.Days[currentState.CurrentDay - 1].WeekInQuarter);
                }
                else
                {
                    if (currentEmployee.AssignedMorningShifts.Count == _month.MonthInQuarter)
                        continue;

                    var shortShiftsToAssign = _quarter.SurplusShifts.Except(currentEmployee.AssignedMorningShifts).
                        OrderBy(s => _random.Next()).ToList();

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
            throw new NotImplementedException();
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
