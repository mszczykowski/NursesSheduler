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
        private readonly Day[] _monthDays;

        private readonly int[,] _shiftCapacities;
        private readonly int[] _morningShiftCapacities;

        private readonly ISolverState _initialSolverState;

        private readonly int _totalNumberOfShiftsToAssign;
        private readonly int _targetMinimalNumberOfNursesOnShift;
        private readonly int _actualMinimalNumberOfNursesOnShift;
        private readonly int _morningShiftsToAssignInMonth;

        private readonly int _numberOfSurplusShifts;

        private int[] _regularDayShiftCapacities;
        private int[] _nightShiftsCapacities;
        private int[] _holidayDayShiftCapacities;

        public ShiftCapacityManager(IEnumerable<INurseState> initialNurseStates, ISolverState initialSolverState,
            DepartamentSettings departamentSettings, Day[] monthDays, Schedule schedule, ScheduleStats scheduleStats,
            IEnumerable<MorningShift> morningShifts)
        {
            _monthDays = monthDays;

            _shiftCapacities = new int[_monthDays.Length, ScheduleConstatns.NumberOfShifts];
            _morningShiftCapacities = new int[_monthDays.Length];

            _initialSolverState = initialSolverState;

            _totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(initialNurseStates, schedule, scheduleStats);
            _targetMinimalNumberOfNursesOnShift = departamentSettings.TargetMinNumberOfNursesOnShift;
            _actualMinimalNumberOfNursesOnShift = GetActualMinimalNumberOfNursesOnShift(departamentSettings
                .TargetMinNumberOfNursesOnShift);

            _morningShiftsToAssignInMonth = GetMorningShiftsToAssignInMonth(initialNurseStates,
                morningShifts, scheduleStats);

            _numberOfSurplusShifts = GetNumberOfSurplusShifts(_actualMinimalNumberOfNursesOnShift);

            _regularDayShiftCapacities = new int[_monthDays.Where(d => d.IsWorkDay).Count()];
            _nightShiftsCapacities = new int[_monthDays.Length];
            _holidayDayShiftCapacities = new int[_monthDays.Length - _regularDayShiftCapacities.Length];

            InitialiseCapacitiesComponents();
        }

        private int GetMorningShiftsToAssignInMonth(IEnumerable<INurseState> initialNurseStates,
            IEnumerable<MorningShift> morningShifts, ScheduleStats scheduleStats)
        {
            var totalNumberOfMorningShifts = morningShifts.Where(m => m.ShiftLength != TimeSpan.Zero).Count() *
                initialNurseStates.Count();

            var numberOfAssignedMorningShifts = initialNurseStates
                .Where(n => n.AssignedMorningShiftIndex != null && morningShifts
                    .First(m => m.Index == n.AssignedMorningShiftIndex).ShiftLength != TimeSpan.Zero)
                .Count()
                +
                initialNurseStates
                .SelectMany(n => n.PreviouslyAssignedMorningShifts)
                .Where(p => morningShifts
                    .First(m => m.Index == p).ShiftLength != TimeSpan.Zero)
                .Count();

            return (totalNumberOfMorningShifts - numberOfAssignedMorningShifts) / 3 - (scheduleStats.MonthInQuarter - 1);
        }

        public void GenerateCapacities(Random random)
        {
            RandomiseShiftCapacities(random);
            InitialiseShiftCapacities();
            InitialisMorningShiftCapacities();
            SubtractInitialState();
        }

        public int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber)
            => _shiftCapacities[dayNumber - 1, (int)shiftIndex];

        public int GetNumberOfNursesForMorningShift(ShiftIndex shiftIndex, int dayNumber)
            => shiftIndex == ShiftIndex.Day ? _morningShiftCapacities[dayNumber - 1] : 0;

        private int GetTotalNumberOfShiftsToAssign(IEnumerable<INurseState> initialNurseStates, Schedule schedule,
            ScheduleStats scheduleStats)
        {
            var totalTimeOffShifts = initialNurseStates.Sum(n => n.NumberOfTimeOffShiftsToAssign)
                +
                schedule.ScheduleNurses
                    .SelectMany(n => n.NurseWorkDays)
                    .Where(wd => wd.IsTimeOff && wd.ShiftType != Domain.Enums.ShiftTypes.Morning
                        && wd.ShiftType != Domain.Enums.ShiftTypes.None)
                    .Count();

            var totalNumberOfShifts =
                (int)Math.Floor(scheduleStats.WorkTimeInMonth * schedule.ScheduleNurses.Count()
                    / ScheduleConstatns.RegularShiftLenght);

            return totalNumberOfShifts - totalTimeOffShifts;
        }

        private int GetActualMinimalNumberOfNursesOnShift(int targetMinimalNumberOfNursesOnShift)
        {
            var calculatedMinimal = _totalNumberOfShiftsToAssign / (_monthDays.Length * ScheduleConstatns.NumberOfShifts);

            return Math.Min(calculatedMinimal, targetMinimalNumberOfNursesOnShift);
        }

        private void InitialiseCapacitiesComponents()
        {
            Array.Fill(_regularDayShiftCapacities, _actualMinimalNumberOfNursesOnShift);
            Array.Fill(_nightShiftsCapacities, _actualMinimalNumberOfNursesOnShift);
            Array.Fill(_holidayDayShiftCapacities, _actualMinimalNumberOfNursesOnShift);

            int surplusShiftsLeft = _numberOfSurplusShifts;

            if (_actualMinimalNumberOfNursesOnShift < _targetMinimalNumberOfNursesOnShift)
            {
                surplusShiftsLeft = AddSurplusShiftsToCapacity(_regularDayShiftCapacities, surplusShiftsLeft);
                surplusShiftsLeft = AddSurplusShiftsToCapacity(_holidayDayShiftCapacities, surplusShiftsLeft);
                AddSurplusShiftsToCapacity(_nightShiftsCapacities, surplusShiftsLeft);
            }
            else
            {
                while (surplusShiftsLeft > 0)
                {
                    surplusShiftsLeft = AddSurplusShiftsToCapacity(_regularDayShiftCapacities, surplusShiftsLeft);
                }
            }
        }

        private void InitialiseShiftCapacities()
        {
            var regularShiftIterator = 0;
            var nightShiftIterator = 0;
            var holidayShiftIterator = 0;

            for (int i = 0; i < _monthDays.Length; i++)
            {
                _shiftCapacities[i, (int)ShiftIndex.Night] = _nightShiftsCapacities[nightShiftIterator++];

                if (!_monthDays[i].IsWorkDay)
                {
                    _shiftCapacities[i, (int)ShiftIndex.Day] = _holidayDayShiftCapacities[holidayShiftIterator++];
                }

                else
                {
                    _shiftCapacities[i, (int)ShiftIndex.Day] = _regularDayShiftCapacities[regularShiftIterator++];
                }
            }
        }

        private void InitialisMorningShiftCapacities()
        {
            var orderedWorkDayIndexes = _monthDays
                .Select(d => d.Date.Day - 1)
                .OrderBy(i => _shiftCapacities[i, (int)ShiftIndex.Day])
                .ToList();

            var morningShiftsLeft = _morningShiftsToAssignInMonth;

            while (morningShiftsLeft > 0)
            {
                for (int i = 0; i < _morningShiftCapacities.Length; i++)
                {
                    _morningShiftCapacities[orderedWorkDayIndexes[i]]++;
                    morningShiftsLeft--;
                    if (morningShiftsLeft == 0)
                    {
                        break;
                    }
                }
            }
        }


        private void RandomiseShiftCapacities(Random random)
        {
            _regularDayShiftCapacities = _regularDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
            _holidayDayShiftCapacities = _holidayDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
            _nightShiftsCapacities = _nightShiftsCapacities.OrderBy(i => random.Next()).ToArray();
        }



        private int GetNumberOfSurplusShifts(int minimalNumberOfNursesOnShift)
        {
            return _totalNumberOfShiftsToAssign
                -
                minimalNumberOfNursesOnShift * _monthDays.Length * ScheduleConstatns.NumberOfShifts;
        }

        private void SubtractInitialState()
        {
            foreach (var entry in _initialSolverState.ScheduleState)
            {
                for (int i = 0; i < entry.Value.Length; i++)
                {
                    if (entry.Value[i] == ShiftTypes.Day)
                    {
                        DecraseCapacity(i, ShiftIndex.Day);
                    }
                    else if (entry.Value[i] == ShiftTypes.Night)
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
                _regularDayShiftCapacities[i]++;
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
