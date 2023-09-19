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
            _result.Add(new ShiftBreakConstraint(departamentSettings.MinimalShiftBreak));
            return this;
        }

        public IConstraintsBuilder AddMaxTotalHoursInWeekConstraintConstraint(DepartamentSettings departamentSettings,
            IEnumerable<DayNumbered> monthDays)
        {
            _result.Add(new MaxTotalHoursInWeekConstraint(departamentSettings.MaximumWeekWorkTimeLength, monthDays));
            return this;
        }

        public IConstraintsBuilder AddAvoidTwoNightShiftsInTheRow()
        {
            _result.Add(new AvoidTwoNightShiftsInTheRow());
            return this;
        }

        public IEnumerable<IConstraint> GetResult() => _result;
    }
}
