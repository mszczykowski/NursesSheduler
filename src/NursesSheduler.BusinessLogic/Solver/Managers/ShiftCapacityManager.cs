using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Solver.Managers
{
    internal sealed class ShiftCapacityManager : IShiftCapacityManager
    {
        private readonly DepartamentSettings _departamentSettings;
        private readonly Day[] _month;
        private readonly ICollection<INurseState> _nurses;
        private readonly Random _random;

        private int[,] _shiftCapacities;
        private int[] _morningShiftCapacities;

        public ShiftCapacityManager(DepartamentSettings departamentSettings, Day[] month, ICollection<INurseState> nurses,
            Random random)
        {
            _departamentSettings = departamentSettings;
            _month = month;
            _nurses = nurses;
            _random = random;

            _shiftCapacities = new int[_month.Length, GeneralConstants.NumberOfShifts];
            _morningShiftCapacities = new int[_month.Length];
        }

        public void InitialiseShiftCapacities()
        {
            var totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(_nurses);

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

            int[,] shortShifts = new int[regularDays.Count < _nurses.Count ? _nurses.Count : regularDays.Count, 2];

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                if (i > shortShifts.GetLength(0)) break;
                shortShifts[i, 0] = regularDays[i];
            }
            for (int i = 0; i < _nurses.Count; i++)
            {
                shortShifts[i % shortShifts.Length, 1]++;
            }

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                _morningShiftCapacities[shortShifts[i, 0]] = shortShifts[i, 1];
            }
        }

        public int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber)
        {
            return _shiftCapacities[dayNumber - 1, (int)shiftIndex];
        }

        public int GetNumberOfNursesForMorningShift(int dayNumber)
        {
            return _morningShiftCapacities[dayNumber - 1];
        }

        private int GetTotalNumberOfShiftsToAssign(ICollection<INurseState> nurses)
        {
            int totalNumberOfShifts = 0;
            foreach (var nurse in nurses)
            {
                totalNumberOfShifts += (int)Math.Floor(nurse.WorkTimeToAssign / GeneralConstants.RegularShiftLenght);
            }
            return totalNumberOfShifts;
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
    }
}
