using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        int RegularShiftsToSwapForMorning { get; }
        void GenerateCapacities(Random random);
        int GetNumberOfNursesForMorningShift(ShiftIndex shiftIndex, int dayNumber);
        int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber);
    }
}