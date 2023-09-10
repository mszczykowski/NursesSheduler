using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.Managers
{
    internal sealed class ShiftCapacityManager : IShiftCapacityManager
    {
        public bool IsSwappingRegularForMorningSuggested { get; init; }
        public bool IsSwappingRequired { get; init; }

        private readonly Day[] _monthDays;
        private readonly int[,] _shiftCapacities;
        private readonly ISolverState _initialSolverState;

        private readonly int _totalNumberOfShiftsToAssign;
        private readonly int _targetMinimalNumberOfNursesOnShift;
        private readonly int _actualMinimalNumberOfNursesOnShift;
        private readonly int _numberOfShiftsPerNurse;
        private readonly int _previousScheduleNumberOfDayShiftNurses;

        private readonly int _numberOfSurplusShifts;
        private int _numberOfSurplusNightShifts;

        private int[] _regularDayShiftCapacities;
        private int[] _holidayDayShiftCapacities;

        public int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber)
            => _shiftCapacities[dayNumber - 1, (int)shiftIndex];

        public ShiftCapacityManager(IEnumerable<INurseState> initialNurseStates, ISolverState initialSolverState,
            DepartamentSettings departamentSettings, Day[] monthDays, Schedule schedule, ScheduleStats scheduleStats,
            IEnumerable<MorningShift> morningShifts, int numberOfShiftsPerNurse)
        {
            _monthDays = monthDays;
            _numberOfShiftsPerNurse = numberOfShiftsPerNurse;

            _shiftCapacities = new int[_monthDays.Length, ScheduleConstatns.NumberOfShifts];

            _initialSolverState = initialSolverState;

            _totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(initialNurseStates, schedule);

            IsSwappingRequired = morningShifts.SumTimeSpan(m => m.ShiftLength) > ScheduleConstatns.RegularShiftLength;
            IsSwappingRegularForMorningSuggested = GetIsSwappingRegularForMorningSuggested(scheduleStats, 
                numberOfShiftsPerNurse);

            _targetMinimalNumberOfNursesOnShift = departamentSettings.TargetMinNumberOfNursesOnShift;
            _actualMinimalNumberOfNursesOnShift = GetActualMinimalNumberOfNursesOnShift();

            _numberOfSurplusShifts = GetNumberOfSurplusShifts(_actualMinimalNumberOfNursesOnShift);

            _regularDayShiftCapacities = new int[_monthDays.Where(d => d.IsWorkDay).Count()];
            _holidayDayShiftCapacities = new int[_monthDays.Length - _regularDayShiftCapacities.Length];

            _previousScheduleNumberOfDayShiftNurses = _initialSolverState.NurseStates
                .Where(n => n.PreviousMonthLastShift == ShiftTypes.Day)
                .Count();

            InitialiseCapacitiesComponents();
        }

        public void GenerateCapacities(Random random)
        {
            RandomiseShiftCapacities(random);
            InitialiseShiftCapacities(random);
            SubtractInitialState();
        }

        private int GetTotalNumberOfShiftsToAssign(IEnumerable<INurseState> initialNurseStates, Schedule schedule)
        {
            var totalTimeOffShifts = initialNurseStates.Sum(n => n.NumberOfTimeOffShiftsToAssign)
                +
                schedule.ScheduleNurses
                    .SelectMany(n => n.NurseWorkDays)
                    .Where(wd => wd.IsTimeOff && wd.ShiftType != ShiftTypes.Morning
                        && wd.ShiftType != ShiftTypes.None)
                    .Count();

            var totalNumberOfShifts = _numberOfShiftsPerNurse * schedule.ScheduleNurses.Count();

            totalNumberOfShifts -= totalTimeOffShifts;

            return totalNumberOfShifts;
        }

        private bool GetIsSwappingRegularForMorningSuggested(ScheduleStats scheduleStats, int numberOfRegularShiftsPerNurse)
        {
            if (scheduleStats.MonthInQuarter == 3 ||
                scheduleStats.WorkTimeInMonth - numberOfRegularShiftsPerNurse * ScheduleConstatns.RegularShiftLength 
                < TimeSpan.Zero)
            {
                return true;
            }

            return false;
        }

        private int GetActualMinimalNumberOfNursesOnShift()
        {
            return _totalNumberOfShiftsToAssign / (_monthDays.Length * ScheduleConstatns.NumberOfShifts);
        }

        private void InitialiseCapacitiesComponents()
        {
            Array.Fill(_regularDayShiftCapacities, _actualMinimalNumberOfNursesOnShift);
            Array.Fill(_holidayDayShiftCapacities, _actualMinimalNumberOfNursesOnShift);

            int surplusShiftsLeft = _numberOfSurplusShifts;

            _numberOfSurplusNightShifts = 0;

            surplusShiftsLeft = AddSurplusShiftsToCapacity(_regularDayShiftCapacities, surplusShiftsLeft);
            surplusShiftsLeft = AddSurplusShiftsToCapacity(_holidayDayShiftCapacities, surplusShiftsLeft);

            _numberOfSurplusNightShifts += surplusShiftsLeft;
        }

        private void InitialiseShiftCapacities(Random random)
        {
            var regularShiftIterator = 0;
            var holidayShiftIterator = 0;

            var surplusNightShiftsLeft = _numberOfSurplusNightShifts;

            for (int i = 0; i < _monthDays.Length; i++)
            {
                _shiftCapacities[i, (int)ShiftIndex.Night] = _actualMinimalNumberOfNursesOnShift;

                if (!_monthDays[i].IsWorkDay)
                {
                    _shiftCapacities[i, (int)ShiftIndex.Day] = _holidayDayShiftCapacities[holidayShiftIterator++];
                }

                else
                {
                    _shiftCapacities[i, (int)ShiftIndex.Day] = _regularDayShiftCapacities[regularShiftIterator++];
                }
            }

            foreach(var day in _monthDays.OrderBy(d => random.Next()))
            {
                if (surplusNightShiftsLeft == 0)
                {
                    break;
                }

                var currentIndex = day.Date.Day - 1;

                if ((currentIndex == 0 &&
                    _shiftCapacities[currentIndex, (int)ShiftIndex.Night] < _previousScheduleNumberOfDayShiftNurses)
                    ||
                    (currentIndex == 0 && _previousScheduleNumberOfDayShiftNurses == 0 &&
                    _shiftCapacities[currentIndex, (int)ShiftIndex.Night] < _targetMinimalNumberOfNursesOnShift)
                    ||
                    (currentIndex != 0 &&
                    _shiftCapacities[currentIndex, (int)ShiftIndex.Night] < _shiftCapacities[currentIndex - 1, (int)ShiftIndex.Day]))
                {
                    _shiftCapacities[currentIndex, (int)ShiftIndex.Night]++;
                    surplusNightShiftsLeft--;
                }
            }
        }

        private void RandomiseShiftCapacities(Random random)
        {
            _regularDayShiftCapacities = _regularDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
            _holidayDayShiftCapacities = _holidayDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
        }
        private int GetNumberOfSurplusShifts(int minimalNumberOfNursesOnShift)
        {
            var result = _totalNumberOfShiftsToAssign
                -
                minimalNumberOfNursesOnShift * _monthDays.Length * ScheduleConstatns.NumberOfShifts;

            return result > 0 ? result : 0;
        }

        private void SubtractInitialState()
        {
            foreach(var nurseState in _initialSolverState.NurseStates)
            {
                for (int i = 0; i < nurseState.ScheduleRow.Length; i++)
                {
                    if(nurseState.TimeOff[i])
                    {
                        continue;
                    }

                    if (nurseState.ScheduleRow[i] == ShiftTypes.Day)
                    {
                        DecraseCapacity(i, ShiftIndex.Day);
                    }
                    else if (nurseState.ScheduleRow[i] == ShiftTypes.Night)
                    {
                        DecraseCapacity(i, ShiftIndex.Night);
                    }
                }
            }
        }

        private void DecraseCapacity(int dayIndex, ShiftIndex shift)
        {
            _shiftCapacities[dayIndex, (int)shift]--;
            if (_shiftCapacities[dayIndex, (int)shift] < 0)
            {
                _shiftCapacities[dayIndex, (int)shift] = 0;
                DecreaseMaxCapacity();
            }
        }

        private void DecreaseMaxCapacity()
        {
            var maxValue = (from int v in _shiftCapacities select v).Max();

            if (maxValue == 0)
            {
                return;
            }

            for (int i = 0; i < _shiftCapacities.GetLength(0); i++)
            {
                for (int j = 0; j < _shiftCapacities.GetLength(1); j++)
                {
                    if (_shiftCapacities[i, j] == maxValue)
                    {
                        _shiftCapacities[i, j]--;
                        return;
                    }
                }
            }
        }

        private int AddSurplusShiftsToCapacity(int[] capacities, int surplusShiftsCount)
        {
            if (surplusShiftsCount <= 0)
            {
                return surplusShiftsCount;
            }
            for (int i = 0; i < capacities.Length; i++)
            {
                capacities[i]++;
                surplusShiftsCount--;
                if (surplusShiftsCount == 0)
                {
                    break;
                }

                _numberOfSurplusNightShifts++;
                surplusShiftsCount--;
                if (surplusShiftsCount == 0)
                {
                    break;
                }
            }
            return surplusShiftsCount;
        }
    }
}
