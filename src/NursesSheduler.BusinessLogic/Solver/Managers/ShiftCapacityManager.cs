using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.Managers
{
    internal sealed class ShiftCapacityManager : IShiftCapacityManager
    {
        public bool IsSwappingRegularForMorningSuggested { get; init; }
        public bool IsSwappingRequired { get; init; }
        public int TargetMinimalNumberOfNursesOnShift { get; init; }
        public int ActualMinimalNumberOfNursesOnShift { get; init; }

        private readonly int _totalNumberOfShiftsToAssign;

        public ShiftCapacityManager(IEnumerable<INurseState> initialNurseStates,
            DepartamentSettings departamentSettings, Day[] monthDays, ScheduleStats scheduleStats,
            IEnumerable<MorningShift> morningShifts, int numberOfShiftsPerNurse)
        {
            _totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(initialNurseStates);
            IsSwappingRequired = morningShifts.SumTimeSpan(m => m.ShiftLength) > ScheduleConstatns.RegularShiftLength;
            IsSwappingRegularForMorningSuggested = GetIsSwappingRegularForMorningSuggested(scheduleStats, 
                numberOfShiftsPerNurse);
            TargetMinimalNumberOfNursesOnShift = departamentSettings.TargetMinNumberOfNursesOnShift;
            ActualMinimalNumberOfNursesOnShift = GetActualMinimalNumberOfNursesOnShift(monthDays.Length);
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

        private int GetActualMinimalNumberOfNursesOnShift(int numberOfDaysInMonth)
        {
            return Math.Min(_totalNumberOfShiftsToAssign / (numberOfDaysInMonth * ScheduleConstatns.NumberOfShifts), 
                TargetMinimalNumberOfNursesOnShift);
        }
    }
}
