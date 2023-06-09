using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber);
        int GetNumberOfNursesForMorningShift(int dayNumber);
        void InitialiseShiftCapacities();
    }
}