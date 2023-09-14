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
        public int TargetMinimalNumberOfNursesOnShift { get; init; }

        private readonly int _totalNumberOfShiftsToAssign;
        private int[,] _shiftCapacities;

        public int GetNumberOfNursesForShift(ShiftIndex shift, int dayNumber) 
            => _shiftCapacities[(int)shift, dayNumber - 1];

        public ShiftCapacityManager(IEnumerable<INurseState> initialNurseStates,
            DepartamentSettings departamentSettings, Day[] monthDays, ScheduleStats scheduleStats,
            IEnumerable<MorningShift> morningShifts, int numberOfShiftsPerNurse)
        {
            _totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(initialNurseStates);

            IsSwappingRequired = morningShifts.SumTimeSpan(m => m.ShiftLength) > ScheduleConstatns.RegularShiftLength;
            IsSwappingRegularForMorningSuggested = GetIsSwappingRegularForMorningSuggested(scheduleStats, 
                numberOfShiftsPerNurse);

            TargetMinimalNumberOfNursesOnShift = departamentSettings.TargetMinNumberOfNursesOnShift;
            CalculateShiftCapacities(monthDays.Length, initialNurseStates);
        }

        private int GetTotalNumberOfShiftsToAssign(IEnumerable<INurseState> initialNurseStates)
        {
            return initialNurseStates.Select(s => s.NumberOfRegularShiftsToAssign).Sum();
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

        private void CalculateShiftCapacities(int numberOfDaysInMonth, IEnumerable<INurseState> nurseStates)
        {
            var numberOfNursesPerDay = Math.Min(_totalNumberOfShiftsToAssign / (numberOfDaysInMonth * ScheduleConstatns.NumberOfShifts),
                TargetMinimalNumberOfNursesOnShift);

            _shiftCapacities = new int[ScheduleConstatns.NumberOfShifts, numberOfDaysInMonth];
            
            for(int i = 0; i < ScheduleConstatns.NumberOfShifts; i++)
            {
                for(int j = 0; j < numberOfDaysInMonth; j++)
                {
                    _shiftCapacities[i, j] = numberOfNursesPerDay - nurseStates
                        .Where(n => (n.ScheduleRow[j] == ShiftTypes.Day && i == (int)ShiftIndex.Day) ||
                            (n.ScheduleRow[j] == ShiftTypes.Night && i == (int)ShiftIndex.Night)).Count();

                    _shiftCapacities[i, j] = _shiftCapacities[i, j] >= 0 ? _shiftCapacities[i, j] : 0;
                }
            } 
        }
    }
}
