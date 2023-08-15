using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver
{
    internal interface IScheduleSolver
    {
        void InitialiseSolver(IEnumerable<MorningShift> morningShifts, DayNumbered[] monthDays,
            IEnumerable<IConstraint> constraints, DepartamentSettings departamentSettings,
            IShiftCapacityManager shiftCapacityManager, ISolverState initialState,
            CancellationToken cancellationToken);
        ISolverState? TrySolveSchedule(Random random);
    }
}
