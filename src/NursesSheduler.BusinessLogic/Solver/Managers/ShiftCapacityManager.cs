using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Managers
{
    internal sealed class ShiftCapacityManager : IShiftCapacityManager
    {
        private readonly DepartamentSettings _departamentSettings;
        private readonly Day[] _month;
        private readonly int _numberOfNurses;
        private readonly TimeSpan _timeToAssignInMonth;
        private readonly Random _random;

        private readonly int[,] _shiftCapacities;
        private readonly int[] _morningShiftCapacities;

        public ShiftCapacityManager(DepartamentSettings departamentSettings, Day[] month, int numberOfNurses,
            TimeSpan timeToAssignInMonth, Random random)
        {
            _departamentSettings = departamentSettings;
            _month = month;
            _numberOfNurses = numberOfNurses;
            _timeToAssignInMonth = timeToAssignInMonth;
            _random = random;

            _shiftCapacities = new int[_month.Length, GeneralConstants.NumberOfShifts];
            _morningShiftCapacities = new int[_month.Length];
        }

        public void InitialiseShiftCapacities(ISolverState initialState)
        {
            var totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign();

            var minimalNumberOfWorkersOnShift = GetMinimalNumberOfNursesOnShift(totalNumberOfShiftsToAssign);

            var numberOfSurplusShifts = GetNumberOfSurplusShifts(totalNumberOfShiftsToAssign,
                minimalNumberOfWorkersOnShift);

            var numberOfWorkingDays = _month.Count(d => d.IsWorkingDay);

            var regularDayShiftCapacities = new int[numberOfWorkingDays];
            var nightShiftsCapacities = new int[_month.Length];
            var holidayDayShiftCapacities = new int[_month.Length - numberOfWorkingDays];

            Array.Fill(regularDayShiftCapacities, minimalNumberOfWorkersOnShift);
            Array.Fill(nightShiftsCapacities, minimalNumberOfWorkersOnShift);
            Array.Fill(holidayDayShiftCapacities, minimalNumberOfWorkersOnShift);

            if (minimalNumberOfWorkersOnShift < _departamentSettings.TargetNumberOfNursesOnShift)
            {
                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < regularDayShiftCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }

                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < holidayDayShiftCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }

                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < nightShiftsCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }
            }
            else
            {
                int i = 0;
                while (numberOfSurplusShifts > 0)
                {
                    regularDayShiftCapacities[i++]++;
                    numberOfSurplusShifts--;
                    if (i >= regularDayShiftCapacities.Length)
                        i = 0;
                }
            }

            regularDayShiftCapacities = regularDayShiftCapacities.OrderBy(i => _random.Next()).ToArray();
            holidayDayShiftCapacities = holidayDayShiftCapacities.OrderBy(i => _random.Next()).ToArray();
            nightShiftsCapacities = nightShiftsCapacities.OrderBy(i => _random.Next()).ToArray();

            int regularShiftIterator = 0;
            int nightShiftIterator = 0;
            int holidayShiftIterator = 0;

            List<int> regularDays = new List<int>();

            for (int i = 0; i < _month.Length; i++)
            {
                _shiftCapacities[i, (int)ShiftIndex.Night] = nightShiftsCapacities[nightShiftIterator++];

                if (!_month[i].IsWorkingDay)
                    _shiftCapacities[i, (int)ShiftIndex.Day] = holidayDayShiftCapacities[holidayShiftIterator++];

                else
                {
                    _shiftCapacities[i, (int)ShiftIndex.Day] = regularDayShiftCapacities[regularShiftIterator++];
                    regularDays.Add(i);
                }
            }
            regularDays.OrderBy(i => _shiftCapacities[i, (int)ShiftIndex.Day]);

            int[,] shortShifts = new int[regularDays.Count < _numberOfNurses ? _numberOfNurses : regularDays.Count, 2];

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                if (i > shortShifts.GetLength(0)) break;
                shortShifts[i, 0] = regularDays[i];
            }
            for (int i = 0; i < _numberOfNurses; i++)
            {
                shortShifts[i % shortShifts.Length, 1]++;
            }

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                _morningShiftCapacities[shortShifts[i, 0]] = shortShifts[i, 1];
            }

            SubtractInitialState(initialState);
        }

        public int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber)
        {
            return _shiftCapacities[dayNumber - 1, (int)shiftIndex];
        }

        public int GetNumberOfNursesForMorningShift(ShiftIndex shiftIndex, int dayNumber)
        {
            if(shiftIndex == ShiftIndex.Day)
            {
                return _morningShiftCapacities[dayNumber - 1];
            }

            return 0;
        }

        private int GetTotalNumberOfShiftsToAssign()
        {
            return (int)Math.Floor((_numberOfNurses * _timeToAssignInMonth) / GeneralConstants.RegularShiftLenght);
        }

        private int GetMinimalNumberOfNursesOnShift(int totalNumberOfShiftsToAssign)
        {
            int calculatedMinimal = totalNumberOfShiftsToAssign / (_month.Length * GeneralConstants.NumberOfShifts);

            return _departamentSettings.TargetNumberOfNursesOnShift < calculatedMinimal ?
                _departamentSettings.TargetNumberOfNursesOnShift : calculatedMinimal;
        }

        private int GetNumberOfSurplusShifts(int totalNumberOfShiftsToAssign, int minimalNumberOfNursesOnShift)
        {
            return totalNumberOfShiftsToAssign - minimalNumberOfNursesOnShift * _month.Length * 2;
        }

        private void SubtractInitialState(ISolverState initialState)
        {
            for(int i = 0; i < initialState.ScheduleState.GetLength(0); i++)
            {
                for(int j = 0; j < initialState.ScheduleState.GetLength(1); j++)
                {
                    if(initialState.ScheduleState[i, j] == null)
                    {
                        continue;
                    }
                    
                    _shiftCapacities[i, j] -= initialState.ScheduleState[i, j].Count;

                    while (_shiftCapacities[i, j] < 0)
                    {
                        _shiftCapacities[i, j]++;
                        DecreaseMaxCapacity();
                    }
                }
            }

            var assignedNurses = new List<int>();

            for (int i = 0; i < initialState.MorningShiftsState.Length; i++)
            {
                if(initialState.MorningShiftsState[i] == null)
                {
                    continue;
                }

                assignedNurses.AddRange(initialState.MorningShiftsState[i]);
            }

            for (int i = 0; i < assignedNurses.Distinct().Count(); i++)
            {
                DecreaseMaxCapacityMorning();
            }
        }

        private void DecreaseMaxCapacity()
        {
            var maxValue = (from int v in _shiftCapacities select v).Max();

            if (maxValue == 0) return;

            for (int i = 0; i < _shiftCapacities.GetLength(0); i++)
            {
                for (int j = 0; j < _shiftCapacities.GetLength(1); j++)
                {
                    if(_shiftCapacities[i, j] == maxValue)
                    {
                        _shiftCapacities[i, j]--;
                        return;
                    }
                }
            }
        }

        private void DecreaseMaxCapacityMorning()
        {
            var maxValue = (from int v in _morningShiftCapacities select v).Max();

            if (maxValue == 0) return;

            for (int i = 0; i < _morningShiftCapacities.Length; i++)
            {
                if (_morningShiftCapacities[i] == maxValue)
                {
                    _morningShiftCapacities[i]--;
                    return;
                }
            }
        }
    }
}
