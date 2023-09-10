using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        public bool IsSwappingRegularForMorningSuggested { get; }
        public bool IsSwappingRequired { get; }
        void GenerateCapacities(Random random);
        int GetNumberOfNursesForRegularShift(ShiftIndex shiftIndex, int dayNumber);
    }
}