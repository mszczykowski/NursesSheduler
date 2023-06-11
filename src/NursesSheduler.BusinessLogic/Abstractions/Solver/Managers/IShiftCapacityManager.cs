using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber);
        int GetNumberOfNursesForMorningShift(ShiftIndex shiftIndex, int dayNumber);
        void InitialiseShiftCapacities(ISolverState initialState);
    }
}