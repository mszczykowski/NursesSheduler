using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface ISolverState
    {
        int CurrentDay { get; set; }
        ShiftIndex CurrentShift { get; }
        bool IsShiftAssigned { get; }
        ICollection<INurseState> NurseStates { get; }
        int NursesToAssignForCurrentShift { get; set; }

        void AdvanceShiftAndDay();
        IEnumerable<int> GetPreviousDayDayShift();
        void PopulateScheduleFromState(Schedule schedule);
        void RecalculateNursesFromAndToShiftHours();
        void SetNursesToAssignCounts(IShiftCapacityManager shiftCapacityManager);
    }
}