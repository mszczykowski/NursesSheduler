using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class ConstraintsDirector : IConstraintsDirector
    {
        public ICollection<IConstraint> GetAllConstraints(DepartamentSettings departamentSettings)
        {
            var constraintsBuilder = new ConstraintsBuilder(departamentSettings);

            return constraintsBuilder
                .AddMaxTotalHoursInWeekConstraintConstraint()
                .AddBreakConstraint()
                .AddHasShiftsToAssignLeftConstraint()
                .GetResult();
        }
    }
}
