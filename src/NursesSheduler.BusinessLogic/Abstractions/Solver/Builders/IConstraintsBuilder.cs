using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface IConstraintsBuilder
    {
        IConstraintsBuilder AddBreakConstraint();
        IConstraintsBuilder AddHasShiftsToAssignLeftConstraint();
        IConstraintsBuilder AddMaxTotalHoursInWeekConstraintConstraint();
        ICollection<IConstraint> GetResult();
    }
}
