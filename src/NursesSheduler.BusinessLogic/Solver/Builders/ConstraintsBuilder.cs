using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Solver.Constraints;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Solver.Builders
{
    internal sealed class ConstraintsBuilder : IConstraintsBuilder
    {
        private readonly ICollection<IConstraint> _result;
        private readonly DepartamentSettings _departamentSettings;

        public ConstraintsBuilder(DepartamentSettings departamentSettings)
        {
            _departamentSettings = departamentSettings;
            _result = new List<IConstraint>();
        }

        public IConstraintsBuilder AddBreakConstraint()
        {
            _result.Add(new BreakConstraint(_departamentSettings.MinmalShiftBreak));
            return this;
        }

        public IConstraintsBuilder AddHasShiftsToAssignLeftConstraint()
        {
            _result.Add(new HasShiftsToAssignLeft());
            return this;
        }

        public IConstraintsBuilder AddMaxTotalHoursInWeekConstraintConstraint()
        {
            _result.Add(new MaxTotalHoursInWeekConstraint(_departamentSettings.MaximalWeekWorkingTime));
            return this;
        }

        public ICollection<IConstraint> GetResult() => _result;
    }
}
