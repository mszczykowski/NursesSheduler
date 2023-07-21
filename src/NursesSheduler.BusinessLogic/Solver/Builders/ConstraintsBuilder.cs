using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Solver.Constraints;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Builders
{
    internal sealed class ConstraintsBuilder : IConstraintsBuilder
    {
        private readonly ICollection<IConstraint> _result;

        public ConstraintsBuilder()
        {
            _result = new List<IConstraint>();
        }

        public IConstraintsBuilder AddBreakConstraint(DepartamentSettings departamentSettings)
        {
            _result.Add(new BreakConstraint(departamentSettings.MinmalShiftBreak));
            return this;
        }

        public IConstraintsBuilder AddHasShiftsToAssignLeftConstraint()
        {
            _result.Add(new HasShiftsToAssignLeft());
            return this;
        }

        public IConstraintsBuilder AddMaxTotalHoursInWeekConstraintConstraint(DepartamentSettings departamentSettings,
            IEnumerable<DayNumbered> monthDays)
        {
            _result.Add(new MaxTotalHoursInWeekConstraint(departamentSettings.MaximalWeekWorkTimeLength, monthDays));
            return this;
        }

        public IConstraintsBuilder AddHasEnoughWorkTimeLeftConstraint()
        {
            _result.Add(new HasEnoughWorkTimeLeft());
            return this;
        }

        public ICollection<IConstraint> GetResult() => _result;
    }
}
