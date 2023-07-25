using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.States;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver
{
    internal sealed class ScheduleSolver : IScheduleSolver
    {
        private readonly IEnumerable<IConstraint> _constraints;
        private readonly INurseQueueDirector _employeeQueueDirector;
        private readonly DepartamentSettings _departamentSettings;
        private readonly IWorkTimeService _workTimeService;

        private readonly IEnumerable<MorningShift> _morningShifts;
        private readonly DayNumbered[] _monthDays;

        private readonly bool _useTeams;

        private Random _random;

        private int _currentNurseId;
        private INurseState _currentNurse;

        private IShiftCapacityManager _shiftCapacityManager;

        public ScheduleSolver(IEnumerable<MorningShift> morningShifts, DayNumbered[] monthDays, 
            IEnumerable<IConstraint> constraints, DepartamentSettings departamentSettings, bool useTeams)
        {
            _morningShifts = morningShifts;
            _monthDays = monthDays;
            _departamentSettings = departamentSettings;
            _constraints = constraints;
            _useTeams = useTeams;
            _employeeQueueDirector = new NurseQueueDirector();
        }

        public ISolverState? GenerateSchedule(Random random, IShiftCapacityManager shiftCapacityManager, 
            ISolverState initialState)
        {
            _random = random;

            _shiftCapacityManager = shiftCapacityManager;

            _shiftCapacityManager.InitialiseShiftCapacities(initialState);

            return AssignShift(initialState, null);
        }


        private ISolverState? AssignShift(ISolverState previousState, Queue<int>? previousQueue)
        {
            Queue<int> currentQueue;
            ISolverState currentState = new SolverState(previousState);

            if (previousQueue is null)
            {
                currentQueue = _employeeQueueDirector
                    .GetSortedEmployeeQueue(previousState.CurrentShift, _monthDays[currentState.CurrentDay - 1].IsWorkDay,
                    currentState.GetPreviousDayDayShift(), previousState.NurseStates, _random);
            }
            else currentQueue = new Queue<int>(previousQueue);

            var currentDay = _monthDays[currentState.CurrentDay - 1];


            while (currentQueue.TryDequeue(out _currentNurseId))
            {
                _currentNurse = currentState.NurseStates.First(e => e.NurseId == _currentNurseId);

                if(_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if(_currentNurse.NumberOfTimeOffShiftsToAssign == 0 || 
                        _constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, 
                            ScheduleConstatns.RegularShiftLenght)))
                    {
                        continue;
                    }

                    currentState.AssignNurseOnTimeOff(_currentNurse);

                    _currentNurse.UpdateStateOnTimeOffShiftAssign(currentState.CurrentShift, currentDay, 
                        _departamentSettings, _workTimeService);
                }
                else if (currentState.NursesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, 
                        ScheduleConstatns.RegularShiftLenght)))
                    {
                        continue;
                    }

                    currentState.AssignNurseToRegularShift(_currentNurse);

                    _currentNurse.UpdateStateOnTimeOffShiftAssign(currentState.CurrentShift, currentDay,
                        _departamentSettings, _workTimeService);
                }
                else if(currentState.NursesToAssignForMorningShift > 0)
                {
                    if (_currentNurse.AssignedMorningShiftIndex is not null)
                    {
                        continue;
                    }

                    var possibleMorningShifts = _morningShifts
                        .Where(m => !_currentNurse.PreviouslyAssignedMorningShifts.Contains(m.Index))
                        .OrderBy(s => _random.Next());

                    MorningShift? morningShiftToAssign = null;

                    foreach (var morningShift in possibleMorningShifts)
                    {
                        if (_constraints.All(c => c.IsSatisfied(currentState, _currentNurse, morningShift.ShiftLength)))
                        {
                            morningShiftToAssign = morningShift;
                            break;
                        }
                    }

                    if (morningShiftToAssign is null)
                    {
                        continue;
                    }

                    if(morningShiftToAssign.ShiftLength != TimeSpan.Zero)
                    {
                        currentState.AssignNurseToMorningShift(_currentNurse);
                    }

                    _currentNurse.UpdateStateOnMorningShiftAssign(morningShiftToAssign, currentDay,
                        _departamentSettings, _workTimeService);
                }

                ISolverState result;

                if(currentState.IsShiftAssigned)
                {
                    currentState.AdvanceUnassignedNursesState();
                    
                    currentState.AdvanceShiftAndDay();
                    if (currentState.CurrentDay > _monthDays.Length)
                    {
                        return currentState;
                    }

                    currentState.SetNursesToAssignCounts(_shiftCapacityManager);

                    result = AssignShift(currentState, null);
                }
                else
                {
                    result = AssignShift(currentState, currentQueue);
                }

                if (result is null)
                {
                    currentState = new SolverState(previousState);
                    continue;
                }
                else
                {
                    return result;
                }
            }

            return null;
        }
    }
}
