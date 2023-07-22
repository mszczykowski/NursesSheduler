using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
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

        public ISolverState GenerateSchedule(Random random, IShiftCapacityManager shiftCapacityManager, 
            ISolverState initialState)
        {
            _random = random;

            _shiftCapacityManager = shiftCapacityManager;

            _shiftCapacityManager.InitialiseShiftCapacities(initialState);

            return AssignShift(initialState, new Queue<int>());
        }


        private ISolverState AssignShift(ISolverState previousState, Queue<int>? previousQueue)
        {
            

            Queue<int> currentQueue;
            ISolverState currentState = new SolverState(previousState);

            if (previousQueue is null)
            {
                currentQueue = _employeeQueueDirector
                    .GetSortedEmployeeQueue(previousState.CurrentShift, _monthDays[currentState.CurrentDay - 1].IsWorkDay,
                    currentState.GetPreviousDayShift(), previousState.NurseStates, _random);
            }
            else currentQueue = new Queue<int>(previousQueue);


            //if (previousState.IsShiftAssined)
            //{
            //    previousState.NursesToAssignForCurrentShift = _shiftCapacityManager
            //        .GetNumberOfNursesForRegularShift(previousState.CurrentShift, previousState.CurrentDay);

            //    previousState.NursesToAssignForMorningShift = _shiftCapacityManager
            //        .GetNumberOfNursesForMorningShift(previousState.CurrentShift, previousState.CurrentDay);

            //    previousState.NursesToAssignOnTimeOff = previousState.Nurses
            //        .Count(n => n.TimeOff[previousState.CurrentDay - 1]);
            //}

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

                    currentState.AssignNurseOnTimeOff(_currentNurse,
                            !_monthDays[currentState.CurrentDay - 1].IsWorkDay,
                            _departamentSettings);
                }
                else if (currentState.NursesToAssignForCurrentShift > 0)
                {
                    if (_constraints.Any(c => !c.IsSatisfied(currentState, _currentNurse, 
                        ScheduleConstatns.RegularShiftLenght)))
                    {
                        continue;
                    }

                    currentState.AssignNurseToRegularShift(_currentNurse,
                        !_monthDays[currentState.CurrentDay - 1].IsWorkDay,
                        _departamentSettings);

                    _currentNurse.UpdateStateOnRegularShiftAssign(isHoliday, CurrentShift, WeekInQuarter, departamentSettings,
                            CalculateNurseHoursToNextShift(nurse.NurseId));

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

                    currentState.AssignNurseToMorningShift(_currentNurse, morningShiftToAssign);

                    currentState.AdvanceStateMorningShift();
                }

                //nowy dizeń

                //jak nowy dzień to nowe liczby

                if (currentState.CurrentDay > _monthDays.Length)
                {
                    return currentState;
                }

                var result = AssignShift(currentState, currentQueue);

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
