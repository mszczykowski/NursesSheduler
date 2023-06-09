using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;

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

        private int currentEmployeeId;
        private INurseState currentNurse;

        private readonly IShiftCapacityManager _shiftCapacityManager;

        public ScheduleSolver(List<INurseState> nurses, Quarter quarter, Day[] month, List<IConstraint> constraints,
            DepartamentSettings departamentSettings, Random random)
        {
            _nurses = nurses;
            _quarter = quarter;
            _month = month;
            _departamentSettings = departamentSettings;
            _constraints = constraints;

            _random = random;

            _shiftCapacityManager = new ShiftCapacityManager(_departamentSettings, _month, _nurses, _random);
            _employeeQueueDirector = new NurseQueueDirector(_random);
        }

        public ISolverState Run(int numberOfRetries)
        {
            _shiftCapacityManager.InitialiseShiftCapacities();

            ISolverState result;
            int actualNumberOfRetries = 0;
            do
            {
                var initialState = new SolverState(_nurses, _month.Length, GeneralConstants.NumberOfShifts);
                result = AssignShift(initialState, new Queue<int>());
                actualNumberOfRetries++;
            }
            while(result == null && actualNumberOfRetries <= numberOfRetries);

            return result;
        }


        private ISolverState AssignShift(ISolverState previousState, Queue<int> previuousQueue)
        {
            if (previousState.CurrentDay > _month.Length)
                return previousState;

            Queue<int> currentQueue;

            if (previousState.EmployeesToAssignForCurrentShift == 0 && previousState.ShortShiftsToAssign == 0)
            {
                previousState.EmployeesToAssignForCurrentShift = _shiftCapacityManager
                    .GetNumberOfNursesForRegularShift(previousState.CurrentShift, previousState.CurrentDay);

                previousState.ShortShiftsToAssign = _shiftCapacityManager
                    .GetNumberOfNursesForMorningShift(previousState.CurrentDay);
                
                currentQueue = _employeeQueueDirector
                    .GetSortedEmployeeQueue(previousState.CurrentShift,
                    _month[previousState.CurrentDay - 1].IsWorkingDay,
                    previousState.GetPreviousDayShift(),
                    previousState.Nurses.ToList(),
                    previousState.CurrentDay);
            }
            else currentQueue = new Queue<int>(previuousQueue);

            var currentState = new SolverState(previousState);

            while (currentQueue.TryDequeue(out currentEmployeeId))
            {
                currentNurse = currentState.Nurses.Single(e => e.NurseId == currentEmployeeId);

                if (currentState.EmployeesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, currentNurse, 
                        GeneralConstants.RegularShiftLenght)))
                    {
                        continue;
                    }

                    if (currentNurse.NumberOfRegularShiftsToAssign <= 0)
                        continue;

                    currentState.AssignNurseToRegularShift(currentNurse,
                        !_month[currentState.CurrentDay - 1].IsWorkingDay,
                        _departamentSettings);
                }
                else
                {
                    if (currentNurse.HadMorningShiftAssigned)
                        continue;

                    var morningShiftsToAssign = _quarter.MorningShifts
                        .Where(m => !currentNurse.AssignedMorningShiftsIds.Contains(m.MorningShiftId))
                        .OrderBy(s => _random.Next()).ToList();

                    MorningShift morningShiftToAssign = null;

                    foreach (var morningShift in morningShiftsToAssign)
                    {
                        if (_constraints.All(c => c.IsSatisfied(currentState, currentNurse, morningShift.ShiftLength)))
                        {
                            morningShiftToAssign = morningShift;
                            break;
                        }
                    }

                    if (morningShiftToAssign == null) 
                        continue;

                    currentState.AssignEmployeeToMorningShift(currentNurse, morningShiftToAssign);
                }

                currentState.AdvanceState();

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

        private void AssignTimeOffs(ISolverState initialState, INurseState nurse)
        {
            for (int i = 0; i < _month.Length; i++)
            {

            }


            int ptoStart;
            int ptoEnd;
            for (int i = 0; i < _month.Length; i++)
            {
                if (nurse.TimeOff[i] == true)
                {
                    ptoStart = i + 1;
                    while (nurse.TimeOff[i] == true && nurse.TimeOff.Length > i) i++;
                    ptoEnd = i + 1;
                }
            }
        }
    }
}
