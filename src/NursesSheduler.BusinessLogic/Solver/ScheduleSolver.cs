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
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver
{
    internal sealed class ScheduleSolver : IScheduleSolver
    {
        private readonly IWorkTimeService _workTimeService;
        private readonly ICurrentDateService _currentDateService;
        private readonly ISolverLoggerService _solverLoggerService;

        private IEnumerable<IConstraint> _constraints;
        private DepartamentSettings _departamentSettings;

        private IEnumerable<MorningShift> _morningShifts;
        private DayNumbered[] _monthDays;

        private Random _random;

        private int _currentNurseId;
        private INurseState _currentNurse;

        private IShiftCapacityManager _shiftCapacityManager;

        private ISolverState _initialState;

        private CancellationToken _cancellationToken;
        private DateTime _startTime;

        private bool _solvingCanceled;

        private IEnumerable<MorningShift> _possibleMorningShifts;

        public ScheduleSolver(IWorkTimeService workTimeService, ICurrentDateService currentDateService, 
            ISolverLoggerService solverLoggerService)
        {
            _workTimeService = workTimeService;
            _currentDateService = currentDateService;
            _solverLoggerService = solverLoggerService;
        }

        public void InitialiseSolver(IEnumerable<MorningShift> morningShifts, DayNumbered[] monthDays,
            IEnumerable<IConstraint> constraints, DepartamentSettings departamentSettings,
            IShiftCapacityManager shiftCapacityManager, ISolverState initialState,
            CancellationToken cancellationToken)
        {
            _morningShifts = morningShifts.Where(m => m.ShiftLength > TimeSpan.Zero);
            _monthDays = monthDays;
            _departamentSettings = departamentSettings;
            _constraints = constraints;
            _shiftCapacityManager = shiftCapacityManager;
            _initialState = initialState;
            _cancellationToken = cancellationToken;
        }

        public ISolverState? TrySolveSchedule(Random random)
        {
            _solvingCanceled = false;
            _random = random;
            _shiftCapacityManager.GenerateCapacities(random);

            var stateCopy = new SolverState(_initialState);

            stateCopy.SetNursesToAssignCounts(_shiftCapacityManager);

            _startTime = _currentDateService.GetCurrentDateTime();

            return AssignShift(stateCopy, GetQueue(), true);
        }

        private ISolverState? AssignShift(ISolverState previousState, INursesQueue previousQueue, bool shouldRebuildQueue)
        {
            ISolverState currentState = new SolverState(previousState);
            INursesQueue currentQueue = CopyQueue(previousQueue);

            if (shouldRebuildQueue)
            {
                currentQueue.PopulateQueue(currentState, _monthDays[currentState.CurrentDay - 1]);
            }

            var currentDay = _monthDays[currentState.CurrentDay - 1];
            var isFirstTry = true;

            while (currentQueue.TryDequeue(out _currentNurseId, isFirstTry) && !ShouldCancel())
            {
                _possibleMorningShifts = Enumerable.Empty<MorningShift>();

                isFirstTry = false;
                _currentNurse = currentState.NurseStates.First(e => e.NurseId == _currentNurseId);

                if (!_currentNurse.TimeOff[currentState.CurrentDay - 1] && currentState.NursesToAssignForMorningShift > 0
                    && _currentNurse.AssignedMorningShiftId is null && _currentNurse.PreviouslyAssignedMorningShifts.Count()
                    != _morningShifts.Count())
                {
                    _possibleMorningShifts = _morningShifts
                        .Where(m => !_currentNurse.PreviouslyAssignedMorningShifts.Contains(m.MorningShiftId)
                            && _constraints.All(c => c.IsSatisfied(currentState, _currentNurse, m.ShiftLength)));
                }

                if (_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if (_currentNurse.NumberOfTimeOffShiftsToAssign == 0 ||
                        _constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse,
                            ScheduleConstatns.RegularShiftLength)))
                    {
                        continue;
                    }

                    currentState.AssignNurseOnTimeOff(_currentNurse);

                    _currentNurse.UpdateStateOnTimeOffShiftAssign(currentState.CurrentShift, currentDay,
                        _departamentSettings, _workTimeService);
                }
                else if (_possibleMorningShifts.Count() > 0 && (!_shiftCapacityManager.IsSwappingRequired ||
                    !_currentNurse.ShouldNurseSwapRegularForMorning ||
                    _currentNurse.CanNurseSwapRegularForMorning(currentState)))
                {
                    var morningShiftToAssign = _possibleMorningShifts.OrderBy(m => _random.Next()).First();

                    if(_shiftCapacityManager.IsSwappingRequired && (_currentNurse.ShouldNurseSwapRegularForMorning ||
                        (_currentNurse.CanNurseSwapRegularForMorning(currentState) 
                        && _shiftCapacityManager.IsSwappingRegularForMorningSuggested)))
                    {
                        currentState.AssignNurseToMorningShift(_currentNurse, true);
                        _currentNurse.UpdateStateOnMorningShiftAssign(morningShiftToAssign, currentDay,
                            _departamentSettings, _workTimeService, true);
                    }
                    else
                    {
                        currentState.AssignNurseToMorningShift(_currentNurse, false);
                        _currentNurse.UpdateStateOnMorningShiftAssign(morningShiftToAssign, currentDay,
                            _departamentSettings, _workTimeService, false);
                    }
                }
                else if (currentState.NursesToAssignForCurrentShift > 0 &&
                    _currentNurse.NumberOfRegularShiftsToAssign > 0 &&
                    _constraints.All(c => c.IsSatisfied(currentState, _currentNurse, ScheduleConstatns.RegularShiftLength)))
                {
                    currentState.AssignNurseToRegularShift(_currentNurse);

                    _currentNurse.UpdateStateOnRegularShiftAssign(currentState.CurrentShift, currentDay,
                        _departamentSettings, _workTimeService);
                }
                else
                {
                    continue;
                }

                ISolverState? result;

                if (currentState.IsShiftAssigned)
                {
                    currentState.AdvanceUnassignedNursesState();
                    currentState.AdvanceShiftAndDay();

                    if (currentState.CurrentDay > _monthDays.Length)
                    {
                        return currentState;
                    }

                    currentState.SetHoursFromLastShift();
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
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        private bool ShouldCancel()
        {
            if(_solvingCanceled)
            {
                return true;
            }

            if(_cancellationToken.IsCancellationRequested)
            {
                _solverLoggerService.LogEvent(SolverEvents.CanceledByUser);
                _solvingCanceled = true;
                return true;
            }

            if ((_currentDateService.GetCurrentDateTime() - _startTime).TotalSeconds >
                _departamentSettings.DefaultGeneratorTimeOut)
            {
                _solverLoggerService.LogEvent(SolverEvents.TimedOut);
                _solvingCanceled = true;
                return true;
            }

            return false;
        }


        private INursesQueue GetQueue()
        {
            if (_departamentSettings.UseTeams)
            {
                return new NursesQueueTeams(_random);
            }

            return new NursesQueue(_random);
        }

        private INursesQueue CopyQueue(INursesQueue currentQueue)
        {
            if(_departamentSettings.UseTeams)
            {
                return new NursesQueueTeams((NursesQueueTeams)currentQueue);
            }

            return new NursesQueue((NursesQueue)currentQueue);
        }
    }
}
