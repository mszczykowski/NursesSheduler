using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        int GetNumberOfNursesForShift(ShiftIndex shiftIndex, int dayNumber);
        void InitialiseShiftCapacities();
    }
}