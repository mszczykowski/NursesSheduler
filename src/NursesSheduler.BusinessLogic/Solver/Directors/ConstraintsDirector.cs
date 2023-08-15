using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class ConstraintsDirector : IConstraintsDirector
    {
        public IEnumerable<IConstraint> GetAllConstraints(DepartamentSettings departamentSettings,
            IEnumerable<DayNumbered> monthDays)
        {
            var constraintsBuilder = new ConstraintsBuilder();

            return constraintsBuilder
                .AddMaxTotalHoursInWeekConstraintConstraint(departamentSettings, monthDays)
                .AddBreakConstraint(departamentSettings)
                .AddHasEnoughWorkTimeLeftConstraint()
                .GetResult();
        }
    }
}
