using NursesScheduler.BusinessLogic.Abstractions.Solver;
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
    internal sealed class ScheduleSolver : IScheduleSolver
    {
        private List<INurseState> _nurses;
        private List<IConstraint> _constraints;
        private INurseQueueDirector _employeeQueueDirector;
        private Random _random;
        private DepartamentSettings _departamentSettings;

        private Quarter _quarter;
        private Day[] _month;

        private int currenNurseId;
        private INurseState currentNurse;

        private readonly IShiftCapacityManager _shiftCapacityManager;

        public ScheduleSolver(List<INurseState> nurses, Quarter quarter, Day[] month, List<IConstraint> constraints,
            DepartamentSettings departamentSettings, Random random, ShiftCapacityManager shiftCapacityManager)
        {
            _nurses = nurses;
            _quarter = quarter;
            _month = month;
            _departamentSettings = departamentSettings;
            _constraints = constraints;

            _random = random;

            _shiftCapacityManager = shiftCapacityManager;
            _employeeQueueDirector = new NurseQueueDirector(_random);
        }

        public ISolverState GenerateSchedule(int numberOfRetries)
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

            if (previousState.NursesToAssignForCurrentShift == 0 && previousState.NursesToAssignForMorningShift == 0)
            {
                previousState.NursesToAssignForCurrentShift = _shiftCapacityManager
                    .GetNumberOfNursesForRegularShift(previousState.CurrentShift, previousState.CurrentDay);

                previousState.NursesToAssignForMorningShift = _shiftCapacityManager
                    .GetNumberOfNursesForMorningShift(previousState.CurrentShift, previousState.CurrentDay);

                previousState.NursesToAssignOnTimeOff = previousState.Nurses
                    .Count(n => n.TimeOff[previousState.CurrentDay - 1]);
                
                currentQueue = _employeeQueueDirector
                    .GetSortedEmployeeQueue(previousState.CurrentShift,
                    _month[previousState.CurrentDay - 1].IsWorkingDay,
                    previousState.GetPreviousDayShift(),
                    previousState.Nurses.ToList(),
                    previousState.CurrentDay);
            }
            else currentQueue = new Queue<int>(previuousQueue);

            var currentState = new SolverState(previousState);

            while (currentQueue.TryDequeue(out currenNurseId))
            {
                currentNurse = currentState.Nurses.Single(e => e.NurseId == currenNurseId);

                if(currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if(currentState.NursesToAssignOnTimeOff <= 0)
                    {
                        continue;
                    }

                    if(currentNurse.NumberOfTimeOffShiftsToAssign > 0 && 
                        _constraints
                            .All(c => c.IsSatisfied(currentState, currentNurse, GeneralConstants.RegularShiftLenght)))
                    {
                        currentState.AssignNurseOnTimeOff(currentNurse,
                            !_month[currentState.CurrentDay - 1].IsWorkingDay,
                            _departamentSettings);
                    }

                    currentState.AdvanceStateTimeOffShift();
                }
                else if (currentState.NursesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, currentNurse, 
                        GeneralConstants.RegularShiftLenght)))
                    {
                        continue;
                    }

                    currentState.AssignNurseToRegularShift(currentNurse,
                        !_month[currentState.CurrentDay - 1].IsWorkingDay,
                        _departamentSettings);

                    currentState.AdvanceStateRegularShift();
                }
                else if(currentState.NursesToAssignForMorningShift > 0)
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

                    currentState.AdvanceStateMorningShift();
                }

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
    }
}
