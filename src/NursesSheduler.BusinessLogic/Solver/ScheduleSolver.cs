using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Queue;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Queue;
using NursesScheduler.BusinessLogic.Solver.States;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver
{
    internal sealed class ScheduleSolver : IScheduleSolver
    {
        private readonly IEnumerable<IConstraint> _constraints;
        private readonly DepartamentSettings _departamentSettings;
        private readonly IWorkTimeService _workTimeService;

        private readonly IEnumerable<MorningShift> _morningShifts;
        private readonly DayNumbered[] _monthDays;

        private Random _random;

        private int _currentNurseId;
        private INurseState _currentNurse;

        private IShiftCapacityManager _shiftCapacityManager;

        private ISolverState _initialState;

        public ScheduleSolver(IEnumerable<MorningShift> morningShifts, DayNumbered[] monthDays, 
            IEnumerable<IConstraint> constraints, DepartamentSettings departamentSettings,
            IShiftCapacityManager shiftCapacityManager, ISolverState initialState,
            IWorkTimeService workTimeService)
        {
            _morningShifts = morningShifts;
            _monthDays = monthDays;
            _departamentSettings = departamentSettings;
            _constraints = constraints;
            _shiftCapacityManager = shiftCapacityManager;
            _initialState = initialState;
            _workTimeService = workTimeService;
        }

        public ISolverState? TrySolveSchedule(Random random)
        {
            _random = random;
            _shiftCapacityManager.GenerateCapacities(random);

            var stateCopy = new SolverState(_initialState);

            stateCopy.SetNursesToAssignCounts(_shiftCapacityManager);

            return AssignShift(stateCopy, GetQueue(), true);
        }


        private ISolverState? AssignShift(ISolverState previousState, INursesQueue previousQueue, bool shouldRebuildQueue)
        {
            ISolverState currentState = new SolverState(previousState);
            INursesQueue currentQueue = CopyQueue(previousQueue, true);

            Console.WriteLine($"NEW STATE: |{currentState.CurrentDay}|{currentState.CurrentShift}|{currentState.NursesToAssignForCurrentShift}");

            if (shouldRebuildQueue)
            {
                Console.WriteLine("BUILDING QUEUE");
                currentQueue.PopulateQueue(currentState, _monthDays[currentState.CurrentDay - 1]);
            }

            var currentDay = _monthDays[currentState.CurrentDay - 1];
            var isFirstTry = true;

            while (currentQueue.TryDequeue(out _currentNurseId, isFirstTry))
            {
                isFirstTry = false;
                _currentNurse = currentState.NurseStates.First(e => e.NurseId == _currentNurseId);

                if(_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if(_currentNurse.NumberOfTimeOffShiftsToAssign == 0 || 
                        _constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, 
                            ScheduleConstatns.RegularShiftLength)))
                    {
                        continue;
                    }

                    currentState.AssignNurseOnTimeOff(_currentNurse);

                    _currentNurse.UpdateStateOnTimeOffShiftAssign(currentState.CurrentShift, currentDay, 
                        _departamentSettings, _workTimeService);
                }
                else if (currentState.NursesToAssignForCurrentShift > 0)
                {
                    if (_currentNurse.NumberOfRegularShiftsToAssign == 0 || 
                        _constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, ScheduleConstatns.RegularShiftLength)))
                    {
                        Console.WriteLine($"NO MATCH, LEFT IN QUEUE: {currentQueue.GetQueueLenght()}");
                        continue;
                    }

                    currentState.AssignNurseToRegularShift(_currentNurse);

                    Console.WriteLine($"ASSIGNED: {_currentNurseId}, LEFT IN QUEUE: {currentQueue.GetQueueLenght()}");

                    _currentNurse.UpdateStateOnRegularShiftAssign(currentState.CurrentShift, currentDay,
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

                    result = AssignShift(currentState, currentQueue, true);
                }
                else
                {
                    result = AssignShift(currentState, currentQueue, false);
                }

                if (result is null)
                {
                    currentState = new SolverState(previousState);
                    Console.WriteLine($"BACK TO STATE: |{previousState.CurrentDay}|{previousState.CurrentShift}|{previousState.NursesToAssignForCurrentShift}");
                    continue;
                }
                else
                {
                    return result;
                }
            }
            Console.WriteLine($"FAILED STATE: |{currentState.CurrentDay}|{currentState.CurrentShift}|{currentState.NursesToAssignForCurrentShift}");
            return null;
        }

        private INursesQueue GetQueue()
        {
            if (_departamentSettings.UseTeams)
            {
                return new NursesQueueTeams(_random);
            }

            return new NursesQueue(_random);
        }

        private INursesQueue CopyQueue(INursesQueue currentQueue, bool shouldCopyInternalQueues)
        {
            if(_departamentSettings.UseTeams)
            {
                return new NursesQueueTeams((NursesQueueTeams)currentQueue, shouldCopyInternalQueues);
            }

            return new NursesQueue((NursesQueue)currentQueue, shouldCopyInternalQueues);
        }
    }
}
