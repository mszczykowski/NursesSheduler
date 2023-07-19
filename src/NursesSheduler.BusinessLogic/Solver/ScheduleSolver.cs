using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver
{
    internal sealed class ScheduleSolver : IScheduleSolver
    {
        private readonly ICollection<IConstraint> _constraints;
        private readonly INurseQueueDirector _employeeQueueDirector;
        private readonly DepartamentSettings _departamentSettings;

        private readonly ICollection<MorningShift> _morningShifts;
        private readonly Day[] _days;

        private Random _random;

        private int _currenNurseId;
        private INurseState _currentNurse;

        private IShiftCapacityManager _shiftCapacityManager;

        public ScheduleSolver(ICollection<MorningShift> morningShifts, Day[] days, ICollection<IConstraint> constraints,
            DepartamentSettings departamentSettings)
        {
            _morningShifts = morningShifts;
            _days = days;
            _departamentSettings = departamentSettings;
            _constraints = constraints;

            _employeeQueueDirector = new NurseQueueDirector();
        }

        public ISolverState GenerateSchedule(Random random, IShiftCapacityManager shiftCapacityManager, 
            ISolverState initialState)
        {
            _random = random;
            _shiftCapacityManager = shiftCapacityManager;

            _shiftCapacityManager.InitialiseShiftCapacities(initialState);

            return AssignShift(initialState, new Queue<int>());
        }


        private ISolverState AssignShift(ISolverState previousState, Queue<int> previuousQueue)
        {
            if (previousState.CurrentDay > _days.Length)
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
                    _days[previousState.CurrentDay - 1].IsWorkingDay,
                    previousState.GetPreviousDayShift(),
                    previousState.Nurses,
                    _random);
            }
            else currentQueue = new Queue<int>(previuousQueue);

            var currentState = new SolverState(previousState);

            while (currentQueue.TryDequeue(out _currenNurseId))
            {
                _currentNurse = currentState.Nurses.Single(e => e.NurseId == _currenNurseId);

                if(_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if(currentState.NursesToAssignOnTimeOff <= 0)
                    {
                        continue;
                    }

                    if(_currentNurse.NumberOfTimeOffShiftsToAssign > 0 && 
                        _constraints
                            .All(c => c.IsSatisfied(currentState, _currentNurse, GeneralConstants.RegularShiftLenght)))
                    {
                        currentState.AssignNurseOnTimeOff(_currentNurse,
                            !_days[currentState.CurrentDay - 1].IsWorkingDay,
                            _departamentSettings);
                    }

                    currentState.AdvanceStateTimeOffShift();
                }
                else if (currentState.NursesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, 
                        GeneralConstants.RegularShiftLenght)))
                    {
                        continue;
                    }

                    currentState.AssignNurseToRegularShift(_currentNurse,
                        !_days[currentState.CurrentDay - 1].IsWorkingDay,
                        _departamentSettings);

                    currentState.AdvanceStateRegularShift();
                }
                else if(currentState.NursesToAssignForMorningShift > 0)
                {
                    if (_currentNurse.HadMorningShiftAssigned)
                        continue;

                    var morningShiftsToAssign = _morningShifts
                        .Where(m => !_currentNurse.AssignedMorningShiftsIds.Contains(m.MorningShiftId))
                        .OrderBy(s => _random.Next()).ToList();

                    MorningShift morningShiftToAssign = null;

                    foreach (var morningShift in morningShiftsToAssign)
                    {
                        if (_constraints.All(c => c.IsSatisfied(currentState, _currentNurse, morningShift.ShiftLength)))
                        {
                            morningShiftToAssign = morningShift;
                            break;
                        }
                    }

                    if (morningShiftToAssign == null) 
                        continue;

                    currentState.AssignEmployeeToMorningShift(_currentNurse, morningShiftToAssign);

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
