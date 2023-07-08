using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISolverService
    {
        ISolverState? SolveSchedule(Schedule currentSchedule, Schedule previousSchedule,
            DepartamentSettings departamentSettings, ICollection<IConstraint> constraints,
            ICollection<NurseQuarterStats> nurseQuarterStats, Random random);
    }
}
