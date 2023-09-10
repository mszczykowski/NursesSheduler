using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Enums;
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

        private int _currentNurseId;
        private INurseState _currentNurse;

        private IShiftCapacityManager _shiftCapacityManager;

        private ISolverState _initialState;

        private CancellationToken _cancellationToken;
        private DateTime _startTime;

        private bool _solvingCanceled;

        private INursesQueueDirector _nursesQueueDirector;

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

            var stateCopy = new SolverState(_initialState);

            stateCopy.SetNursesToAssignCounts(_shiftCapacityManager);

            _nursesQueueDirector = new NursesQueueDirector(random);

            _startTime = _currentDateService.GetCurrentDateTime();

            var result = AssignRegularShift(stateCopy, null);

            if (result is not null)
            {
                TryAssignTimeOffs(result.NurseStates);
                foreach(var nurse in result.NurseStates)
                {
                    nurse.NumberOfRegularShiftsToAssign += nurse.NumberOfTimeOffShiftsToAssign;
                }

                TryAssignAdditionalShifts(result.NurseStates, random);
                TryAssignMorningShifts(result.NurseStates, random);
                
            }

            return result;
        }

        private ISolverState? AssignRegularShift(ISolverState previousState, Queue<int>? previousQueue)
        {
            ISolverState currentState = new SolverState(previousState);
            Queue<int> currentQueue;
            DayNumbered currentDay = _monthDays[currentState.CurrentDay - 1];

            if (previousQueue is null)
            {
                currentQueue = _nursesQueueDirector.BuildSortedNursesQueue(currentState, currentDay);
            }
            else
            {
                currentQueue = new Queue<int>(previousQueue);
            }


            while (currentQueue.TryDequeue(out _currentNurseId) && !ShouldCancel())
            {
                _currentNurse = currentState.NurseStates.First(e => e.NurseId == _currentNurseId);

                if (_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    continue;
                }


                if (_constraints.Any(c => !c.IsSatisfied(currentState.CurrentDay, currentState.CurrentShift, _currentNurse,
                            ScheduleConstatns.RegularShiftLength)))
                {
                    continue;
                }


                if (_currentNurse.NumberOfRegularShiftsToAssign == 0)
                {
                    continue;
                }

                currentState.NursesToAssignForCurrentShift--;
                _currentNurse.UpdateStateOnRegularShiftAssign(currentState.CurrentShift, currentDay,
                    _departamentSettings, _workTimeService);

                ISolverState? result;

                if (currentState.IsShiftAssigned)
                {
                    currentState.AdvanceShiftAndDay();

                    if (currentState.CurrentDay > _monthDays.Length)
                    {
                        return currentState;
                    }

                    currentState.RecalculateNursesFromAndToShiftHours();
                    currentState.SetNursesToAssignCounts(_shiftCapacityManager);

                    result = AssignRegularShift(currentState, null);
                }
                else
                {
                    result = AssignRegularShift(currentState, currentQueue);
                }

                if (result is null)
                {
                    if (currentQueue.Count == 0)
                    {
                        return null;
                    }

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
            if (_solvingCanceled)
            {
                return true;
            }

            if (_cancellationToken.IsCancellationRequested)
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

        private void TryAssignAdditionalShifts(IEnumerable<INurseState> nurseStates, Random random)
        {
            foreach (var nurseState in nurseStates.OrderBy(n => random.Next()))
            {
                var daysOrdered = _monthDays
                    .OrderByDescending(d => nurseStates.Where(n => n.ScheduleRow[d.Date.Day - 1] == ShiftTypes.Day && !n.TimeOff[d.Date.Day - 1]).Count() 
                        < _shiftCapacityManager.TargetMinimalNumberOfNursesOnShift)
                    .ThenByDescending(d => d.IsWorkDay)
                    .ThenBy(d => nurseStates
                        .Where(n => (n.ScheduleRow[d.Date.Day - 1] == ShiftTypes.Day || n.ScheduleRow[d.Date.Day - 1] == ShiftTypes.Morning) && !n.TimeOff[d.Date.Day - 1]).Count());

                foreach (var currentDay in daysOrdered.Select(d => d.Date.Day))
                {
                    var nextDay = currentDay + 1;

                    if (nurseState.NumberOfRegularShiftsToAssign == 0)
                    {
                        break;
                    }

                    if (nurseState.ScheduleRow[currentDay - 1] != ShiftTypes.None && (nextDay > _monthDays.Length ||
                        nurseState.ScheduleRow[nextDay - 1] == ShiftTypes.None))
                    {
                        continue;
                    }
                    if (nextDay <= _monthDays.Length && nurseState.ScheduleRow[nextDay - 1] != ShiftTypes.None)
                    {
                        continue;
                    }

                    nurseState.RecalculateFromPreviousAndToNextShift(currentDay);

                    if (nurseState.TimeOff[currentDay - 1] || _constraints.Any(c => !c.IsSatisfied(currentDay, ShiftIndex.Day,
                         nurseState, ScheduleConstatns.RegularShiftLength)))
                    {
                        continue;
                    }

                    nurseState.UpdateStateOnRegularShiftAssign(ShiftIndex.Day, _monthDays[currentDay - 1],
                        _departamentSettings, _workTimeService);

                    if (nurseState.NumberOfRegularShiftsToAssign != 0 && nextDay <= _monthDays.Length &&
                        nurseState.ScheduleRow[nextDay - 1] == ShiftTypes.None)
                    {
                        nurseState.RecalculateFromPreviousAndToNextShift(nextDay);

                        if (_constraints.Any(c => !c.IsSatisfied(nextDay, ShiftIndex.Night,
                            nurseState, ScheduleConstatns.RegularShiftLength)) || nurseState.TimeOff[nextDay - 1])
                        {
                            continue;
                        }

                        nurseState.UpdateStateOnRegularShiftAssign(ShiftIndex.Night, _monthDays[nextDay - 1],
                            _departamentSettings, _workTimeService);
                    }
                }
            }
        }

        private void TryAssignMorningShifts(IEnumerable<INurseState> nurseStates, Random random)
        {
            foreach (var nurseState in nurseStates)
            {
                var possibleMorningShifts = _morningShifts
                                .Where(m => !nurseState.PreviouslyAssignedMorningShifts.Contains(m.MorningShiftId));

                if (nurseState.AssignedMorningShiftId is not null || possibleMorningShifts.Count() == 0)
                {
                    continue;
                }

                if (_shiftCapacityManager.IsSwappingRequired && _shiftCapacityManager.IsSwappingRegularForMorningSuggested
                    && nurseState.ShouldNurseSwapRegularForMorning)
                {
                    foreach (var day in _monthDays.OrderByDescending(d => nurseStates
                        .Where(n => n.ScheduleRow[d.Date.Day - 1] == ShiftTypes.Day && !n.TimeOff[d.Date.Day - 1]).Count()))
                    {
                        if (nurseState.ScheduleRow[day.Date.Day - 1] == ShiftTypes.Day && !nurseState.TimeOff[day.Date.Day - 1])
                        {
                            nurseState.UpdateStateOnMorningShiftAssign(possibleMorningShifts
                                   .Where(m => m.ShiftLength == possibleMorningShifts.Select(p => p.ShiftLength).Max())
                                   .First(), day, _departamentSettings, _workTimeService);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var dayNumber in _monthDays.OrderBy(d => nurseStates
                        .Where(n => n.ScheduleRow[d.Date.Day - 1] == ShiftTypes.Day && !n.TimeOff[d.Date.Day - 1]).Count())
                        .Select(d => d.Date.Day))
                    {
                        if (nurseState.ScheduleRow[dayNumber - 1] != ShiftTypes.None || nurseState.TimeOff[dayNumber - 1])
                        {
                            continue;
                        }

                        nurseState.RecalculateFromPreviousAndToNextShift(dayNumber);

                        var currentDayPossibleMorningShifts = possibleMorningShifts
                            .Where(m => _constraints.All(c => c.IsSatisfied(dayNumber, ShiftIndex.Day,
                            nurseState, m.ShiftLength)));

                        if (currentDayPossibleMorningShifts.Count() != 0)
                        {
                            nurseState.UpdateStateOnMorningShiftAssign(possibleMorningShifts
                                   .Where(m => m.ShiftLength == possibleMorningShifts.Select(p => p.ShiftLength).Min())
                                   .First(), _monthDays[dayNumber - 1], _departamentSettings, _workTimeService);

                            break;
                        }
                    }
                }
            }
        }

        private void TryAssignTimeOffs(IEnumerable<INurseState> nurseStates)
        {
            foreach (var nurseState in nurseStates.Where(n => n.TimeOff.Any(t => t)))
            {
                var daysOrdered = _monthDays
                    .Where(d => nurseState.TimeOff[d.Date.Day - 1])
                    .OrderBy(d => d.IsWorkDay);

                foreach (var currentDay in daysOrdered.Select(d => d.Date.Day))
                {
                    var nextDay = currentDay + 1;

                    if (nurseState.NumberOfTimeOffShiftsToAssign == 0)
                    {
                        break;
                    }

                    if (nurseState.ScheduleRow[currentDay - 1] != ShiftTypes.None && (nextDay > _monthDays.Length ||
                        nurseState.ScheduleRow[nextDay - 1] == ShiftTypes.None))
                    {
                        continue;
                    }
                    if (nextDay <= _monthDays.Length && nurseState.ScheduleRow[nextDay - 1] != ShiftTypes.None)
                    {
                        continue;
                    }

                    nurseState.RecalculateFromPreviousAndToNextShift(currentDay);

                    if (_constraints.Any(c => !c.IsSatisfied(currentDay, ShiftIndex.Day,
                         nurseState, ScheduleConstatns.RegularShiftLength)))
                    {
                        continue;
                    }

                    nurseState.UpdateStateOnTimeOffShiftAssign(ShiftIndex.Day, _monthDays[currentDay - 1],
                        _departamentSettings, _workTimeService);

                    if (nurseState.NumberOfTimeOffShiftsToAssign != 0 && nextDay <= _monthDays.Length &&
                        nurseState.ScheduleRow[nextDay - 1] == ShiftTypes.None)
                    {
                        nurseState.RecalculateFromPreviousAndToNextShift(nextDay);

                        if (_constraints.Any(c => !c.IsSatisfied(nextDay, ShiftIndex.Night,
                            nurseState, ScheduleConstatns.RegularShiftLength)))
                        {
                            continue;
                        }

                        nurseState.UpdateStateOnTimeOffShiftAssign(ShiftIndex.Night, _monthDays[nextDay - 1],
                            _departamentSettings, _workTimeService);
                    }
                }
            }
        }
    }
}
