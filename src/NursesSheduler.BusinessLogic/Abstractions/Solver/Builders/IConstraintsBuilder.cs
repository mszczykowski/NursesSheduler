using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface IConstraintsBuilder : IBuilder<IEnumerable<IConstraint>>
    {
        IConstraintsBuilder AddBreakConstraint(DepartamentSettings departamentSettings);
        IConstraintsBuilder AddHasEnoughWorkTimeLeftConstraint();
        IConstraintsBuilder AddMaxTotalHoursInWeekConstraintConstraint(DepartamentSettings departamentSettings, 
            IEnumerable<DayNumbered> days);
        IConstraintsBuilder AddAvoidTwoNightShiftsInTheRow();
    }
}
