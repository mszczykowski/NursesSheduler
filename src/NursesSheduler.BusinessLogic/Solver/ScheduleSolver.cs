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
using System.Diagnostics;

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

            _shiftCapacityManager.GenerateCapacities(random);

            var stateCopy = new SolverState(_initialState);

            stateCopy.SetNursesToAssignCounts(_shiftCapacityManager);

            _nursesQueueDirector = new NursesQueueDirector(random);

            _startTime = _currentDateService.GetCurrentDateTime();

            var result = AssignRegularShift(stateCopy, null);

            if (result is not null)
            {
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

                if (_constraints.Any(c => !c.IsSatisfied(currentState.CurrentDay, currentState.CurrentShift, _currentNurse,
                            ScheduleConstatns.RegularShiftLength)))
                {
                    continue;
                }

                if (_currentNurse.TimeOff[currentState.CurrentDay - 1])
                {
                    if (_currentNurse.NumberOfTimeOffShiftsToAssign == 0)
                    {
                        continue;
                    }

                    _currentNurse.UpdateStateOnTimeOffShiftAssign(currentState.CurrentShift, currentDay,
                        _departamentSettings, _workTimeService);
                }
                else
                {
                    if (_currentNurse.NumberOfRegularShiftsToAssign == 0)
                    {
                        continue;
                    }

                    currentState.NursesToAssignForCurrentShift--;
                    _currentNurse.UpdateStateOnRegularShiftAssign(currentState.CurrentShift, currentDay,
                        _departamentSettings, _workTimeService);
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
                    foreach (var day in _monthDays.OrderBy(d => random.Next()))
                    {
                        if(nurseState.ScheduleRow[day.Date.Day - 1] == ShiftTypes.Day && !nurseState.TimeOff[day.Date.Day - 1])
                        {
                            nurseState.AssignMorningShift(day.Date.Day, possibleMorningShifts
                                .Where(m => m.ShiftLength == possibleMorningShifts.Select(p => p.ShiftLength).Max())
                                .First());
                            break;
                        }
                    }
                }
                else
                {
                    nurseState.RecalculateHoursFromLastShift();
                    nurseState.ResetHoursToNextShiftMatrix(_workTimeService);

                    for(int i = 0; i < nurseState.ScheduleRow.Length; i++)
                    {
                        if(nurseState.ScheduleRow[i] != ShiftTypes.None)
                        {
                            nurseState.ResetHoursFromLastShift();
                            continue;
                        }

                        if (nurseState.TimeOff[i])
                        {
                            continue;
                        }

                        var currentDayPossibleMorningShifts = possibleMorningShifts
                            .Where(m => _constraints.All(c => c.IsSatisfied(i + 1, ShiftIndex.Day, 
                            nurseState, m.ShiftLength)));

                        if(currentDayPossibleMorningShifts.Count() != 0)
                        {
                            nurseState.AssignMorningShift(i + 1, possibleMorningShifts
                                   .Where(m => m.ShiftLength == possibleMorningShifts.Select(p => p.ShiftLength).Min())
                                   .First());

                            break;
                        }

                        nurseState.AdvanceState();
                        nurseState.AdvanceState();
                    }
                }
            }
        }
    }
}
