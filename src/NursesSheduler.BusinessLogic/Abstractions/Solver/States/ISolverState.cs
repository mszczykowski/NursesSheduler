using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface ISolverState
    {
        int CurrentDay { get; }
        ShiftIndex CurrentShift { get; }
        bool IsShiftAssigned { get; }
        ICollection<INurseState> NurseStates { get; }
        int NursesToAssignForCurrentShift { get; }
        int NursesToAssignForMorningShift { get; }
        IDictionary<int, ShiftTypes[]> ScheduleState { get; }

        void AdvanceShiftAndDay();
        void AdvanceUnassignedNursesState();
        void AssignNurseOnTimeOff(INurseState nurse);
        void AssignNurseToMorningShift(INurseState nurse);
        void AssignNurseToRegularShift(INurseState nurse);
        HashSet<int> GetPreviousDayDayShift();
        void PopulateScheduleFromState(Schedule schedule);
        void SetNursesToAssignCounts(IShiftCapacityManager shiftCapacityManager);
    }
}