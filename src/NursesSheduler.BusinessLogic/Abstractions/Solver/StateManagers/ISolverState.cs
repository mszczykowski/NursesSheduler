using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface ISolverState
    {
        public int CurrentDay { get; set; }
        public ShiftIndex CurrentShift { get; set; }

        public int NursesToAssignForCurrentShift { get; set; }
        public int NursesToAssignForMorningShift { get; set; }
        public int NursesToAssignOnTimeOff { get; set; }

        public ICollection<INurseState> NurseStates { get; }
        public IDictionary<int, ShiftTypes[]> ScheduleState { get; }
        bool IsShiftAssined { get; }

        void AdvanceStateRegularShift();
        void AdvanceStateMorningShift();
        void AdvanceStateTimeOffShift();
        void AssignEmployeeToMorningShift(INurseState nurse, MorningShift morningShift);
        void AssignNurseToRegularShift(INurseState nurse, bool isHoliday, DepartamentSettings departamentSettings);
        void AssignNurseOnTimeOff(INurseState nurse, bool isHoliday, DepartamentSettings departamentSettings);
        TimeSpan GetHoursToScheduleEnd();
        HashSet<int> GetPreviousDayShift();
        void PopulateScheduleFromState(Schedule schedule);
    }
}